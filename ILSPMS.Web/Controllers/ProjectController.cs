using AutoMapper;
using ILSPMS.Common;
using ILSPMS.Data;
using ILSPMS.Entities;
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

        public ProjectController(IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork,
            IEntityBaseRepository<Project> projectRepository, IEntityBaseRepository<User> userRepository
            ) : base (_errorsRepository, _unitOfWork)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }

        [Route("{filter?}/{all?}")]
        public HttpResponseMessage Get(HttpRequestMessage request, string filter = null, bool all = false)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<Project> projects = null;

                var currentUser = _userRepository.GetSingleByUsername(User.Identity.Name.Trim().ToLower());

                if (currentUser.RoleID == (int)Enumerations.Role.PM)
                {
                    projects = _projectRepository
                        .FindBy(s => !s.Deleted && s.ProjectManagerID == currentUser.ID
                            && (!all || s.DateCreated.Year == DateTime.Now.Year))
                        .OrderBy(m => m.Name)
                        .ToList();
                }
                else if (currentUser.RoleID == (int)Enumerations.Role.DivisionOfficer)
                {
                    projects = _projectRepository
                        .FindBy(s => !s.Deleted && s.DivisionID == currentUser.DivisionID
                            && (!all || s.DateCreated.Year == DateTime.Now.Year))
                        .OrderBy(m => m.Name)
                        .ToList();
                }
                else
                {
                    projects = _projectRepository
                        .FindBy(s => !s.Deleted
                            && (!all || s.DateCreated.Year == DateTime.Now.Year))
                        .OrderBy(m => m.Name)
                        .ToList();
                }

                var list = Mapper.Map<List<Project>, List<ProjectViewModel>>(projects);
                response = request.CreateResponse(HttpStatusCode.OK, new { items = list });

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
                        _projectRepository.Add(project);
                        _unitOfWork.Commit();
                        model.ID = project.ID;
                    }
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = true, item = model });
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
    }
}
