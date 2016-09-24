﻿using AutoMapper;
using ILSPMS.Common;
using ILSPMS.Data;
using ILSPMS.Entities;
using ILSPMS.Services;
using ILSPMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ILSPMS.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/projects")]
    public class ProjectController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Project> _projectRepository;
        private readonly IEntityBaseRepository<User> _userRepository;
        private readonly IEntityBaseRepository<Division> _divisionRepository;
        private readonly IEntityBaseRepository<Milestone> _milestoneRepository;

        private readonly IProjectService _projectService;

        public ProjectController(IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork,
            IEntityBaseRepository<Project> projectRepository, IEntityBaseRepository<User> userRepository,
            IEntityBaseRepository<Division> divisionRepository, IEntityBaseRepository<Milestone> milestoneRepository,
            IProjectService projectService
            ) : base (_errorsRepository, _unitOfWork)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _divisionRepository = divisionRepository;
            _milestoneRepository = milestoneRepository;
            _projectService = projectService;
        }

        [Route("{filter?}/{all?}/{divisionID?}/{forApproval?}")]
        public HttpResponseMessage Get(HttpRequestMessage request, string filter = null, bool all = false, int? divisionID = 0, bool forApproval = false)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<Project> projects = null;

                var currentUser = _userRepository.GetSingleByUsername(User.Identity.Name.Trim().ToLower());
                filter = filter != null ? filter.Trim().ToLower() : null;

                if (forApproval && currentUser.RoleID != (int)Enumerations.Role.PM)
                {
                    projects = _projectRepository
                    .FindBy(s => !s.Deleted && s.ProjectMovements.OrderByDescending(pm => pm.DateCreated).FirstOrDefault() != null
                        && !s.ProjectMovements.OrderByDescending(pm => pm.DateCreated).FirstOrDefault().IsApproved
                        && s.ProjectMovements.OrderByDescending(pm => pm.DateCreated).FirstOrDefault().ApproverRoleID == currentUser.RoleID
                        //&& (filter == null || s.Name.ToLower().Contains(filter) || s.Division.Name.Contains(filter))
                        //&& (!all || s.DateCreated.Year == DateTime.Now.Year)
                        )
                    .OrderBy(m => m.Name)
                    .ToList();
                }
                else
                {
                    if (currentUser.RoleID == (int)Enumerations.Role.PM)
                    {
                        projects = _projectRepository
                            .FindBy(s => !s.Deleted && s.ProjectManagerID == currentUser.ID
                                    && (filter == null || s.Name.ToLower().Contains(filter) || s.Division.Name.Contains(filter))
                                    && (!all || s.DateCreated.Year == DateTime.Now.Year))
                                .OrderBy(m => m.Name)
                                .ToList();
                    }
                    else if (currentUser.RoleID == (int)Enumerations.Role.DivisionChief)
                    {
                        projects = _projectRepository
                            .FindBy(s => !s.Deleted && s.DivisionID == currentUser.DivisionID
                                && (filter == null || s.Name.ToLower().Contains(filter) || s.Division.Name.Contains(filter))
                                && (!all || s.DateCreated.Year == DateTime.Now.Year))
                            .OrderBy(m => m.Name)
                            .ToList();
                    }
                    else
                    {
                        projects = _projectRepository
                            .FindBy(s => !s.Deleted
                                && (divisionID == 0 || s.DivisionID == divisionID)
                                && (filter == null || s.Name.ToLower().Contains(filter) || s.Division.Name.Contains(filter))
                                && (!all || s.DateCreated.Year == DateTime.Now.Year))
                            .OrderBy(m => m.Name)
                            .ToList();
                    }
                }

                var topOrder = _milestoneRepository.GetAll().Max(s => s.Order);

                var list = Mapper.Map<List<Project>, List<ProjectViewModel>>(projects);
                response = request.CreateResponse(HttpStatusCode.OK, new { items = list, ms = topOrder });

                return response;
            });
        }

        [Authorize(Roles = "Admin")]
        [Route("update")]
        [HttpPost]
        public HttpResponseMessage CreateEdit(HttpRequestMessage request, ProjectViewModel model)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var currentUser = _userRepository.GetSingleByUsername(User.Identity.Name.Trim().ToLower());

                if (ModelState.IsValid)
                {
                    if (model.ID > 0)
                    {
                        var project = _projectRepository.GetSingle(model.ID);
                        if (project != null)
                        {
                            project.Name = model.Name;
                            project.DivisionID = model.DivisionID;
                            project.ProjectManagerID = model.ProjectManagerID;
                            project.Budget = model.Budget;
                            _projectRepository.Edit(project);
                            _unitOfWork.Commit();
                        }
                    }
                    else
                    {
                        var project = new Project()
                        {
                            Name = model.Name.Trim(),
                            AddedByID = currentUser.ID,
                            DateCreated = DateTime.Now,
                            Budget = model.Budget,
                            DivisionID = model.DivisionID,
                            ProjectManagerID = model.ProjectManagerID,
                            Deleted = false
                        };
                        project.Initialize();
                        _projectRepository.Add(project);
                        _unitOfWork.Commit();
                        model.ID = project.ID;
                    }
                    var obj = _projectRepository.GetSingle(model.ID);
                    var item = Mapper.Map<ProjectViewModel>(obj);
                    var pm = _userRepository.GetSingle(obj.ProjectManagerID.HasValue ? (int)obj.ProjectManagerID : 0);
                    item.ProjectManager = pm != null ? $"{pm.FirstName} {pm.LastName}" : "";
                    item.DivisionName = _divisionRepository.GetSingle(item.DivisionID).Name;

                    response = request.CreateResponse(HttpStatusCode.OK, new { success = true, item = item });
                }
                else
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });

                return response;
            });
        }

        [Authorize(Roles = "Division Officer")]
        [Route("assignpm")]
        [HttpPost]
        public HttpResponseMessage Assign(HttpRequestMessage request, ProjectViewModel model)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {
                    var project = _projectRepository.GetSingle(model.ID);
                    if (project != null)
                    {
                        project.ProjectManagerID = model.ProjectManagerID;
                    }
                    _unitOfWork.Commit();
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                }
                else
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });

                return response;
            });
        }

        [Authorize(Roles = "Admin")]
        [Route("remove")]
        [HttpPost]
        public HttpResponseMessage Archive(HttpRequestMessage request, ProjectViewModel model)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {
                    var project = _projectRepository.GetSingle(model.ID);
                    if (project != null)
                    {
                        project.Deleted = true;
                    }
                    _unitOfWork.Commit();
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                }
                else
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });

                return response;
            });
        }

        [Authorize(Roles = "Project Manager")]
        [Route("submit")]
        [HttpPost]
        public HttpResponseMessage SubmitProject(HttpRequestMessage request, ProjectViewModel model)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {
                    var project = _projectRepository.GetSingle(model.ID);
                    if (project != null)
                    {
                        _projectService.Submit(project);
                    }
                    _unitOfWork.Commit();

                    var obj = _projectRepository.GetSingle(model.ID);
                    var item = Mapper.Map<ProjectViewModel>(project);

                    response = request.CreateResponse(HttpStatusCode.OK, new { success = true, item = item });
                }
                else
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });

                return response;
            });
        }

    }
}
