using AutoMapper;
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

        [Route("{filter?}/{divisionID?}/{forApproval?}/{year?}")]
        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request, string filter = null, int? divisionID = 0, bool forApproval = false, int year = 0)
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
                    .FindBy(s => !s.Deleted && s.ProjectMovements.OrderByDescending(pm => pm.ID).FirstOrDefault() != null
                        && !s.ProjectMovements.OrderByDescending(pm => pm.ID).FirstOrDefault().IsApproved
                        && (s.ProjectMovements.OrderByDescending(pm => pm.ID).FirstOrDefault().ProjectMovementTypeID == (int)Enumerations.ProjectMovementType.ForApproval
                            || s.ProjectMovements.OrderByDescending(pm => pm.ID).FirstOrDefault().ProjectMovementTypeID == (int)Enumerations.ProjectMovementType.NextApproval)
                        && s.ProjectMovements.OrderByDescending(pm => pm.ID).FirstOrDefault().ApproverRoleID == currentUser.RoleID
                        && (s.DivisionID == currentUser.DivisionID || currentUser.RoleID == (int)Enumerations.Role.DeputyExecDir
                            || currentUser.RoleID == (int)Enumerations.Role.ExecDir)
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
                                    && (year == 0 || s.DateCreated.Year == year)
                                    )
                                .OrderBy(m => m.Name)
                                .ToList();
                    }
                    else if (currentUser.RoleID == (int)Enumerations.Role.DivisionChief)
                    {
                        projects = _projectRepository
                            .FindBy(s => !s.Deleted && (s.DivisionID == currentUser.DivisionID || s.ProjectManagerID == currentUser.ID)
                                && (filter == null || s.Name.ToLower().Contains(filter) || s.Division.Name.Contains(filter))
                                && (year == 0 || s.DateCreated.Year == year)
                                )
                            .OrderBy(m => m.Name)
                            .ToList();
                    }
                    else
                    {
                        projects = _projectRepository
                            .FindBy(s => !s.Deleted
                                && (divisionID == 0 || s.DivisionID == divisionID)
                                && (filter == null || s.Name.ToLower().Contains(filter) || s.Division.Name.Contains(filter))
                                && (year == 0 || s.DateCreated.Year == year)
                                )
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

        [Route("years")]
        [HttpPost]
        public HttpResponseMessage ProjectYears(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var projectYears = _projectRepository.GetAll()
                    .Select(s => s.DateCreated.Year)
                    .Distinct().OrderBy(s => s).ToList();

                response = request.CreateResponse(HttpStatusCode.OK, new { items = projectYears });

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
                    var item = Mapper.Map<NewProjectMovementViewModel>(obj);
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

        [Authorize(Roles = "Division Chief")]
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

        [Authorize(Roles = "Project Manager, Division Chief")]
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

        [Authorize(Roles = "Division Chief, Deputy Executive Director, Executive Director, APD")]
        [Route("approve")]
        [HttpPost]
        public HttpResponseMessage ApproveProject(HttpRequestMessage request, ProjectViewModel model)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {
                    var currentUser = _userRepository.GetSingleByUsername(User.Identity.Name.Trim().ToLower());
                    var project = _projectRepository.GetSingle(model.ID);
                    if (project != null)
                    {
                        project.Approve(currentUser);
                        _unitOfWork.Commit();
                        _projectService.Submit(project);
                    }

                    var obj = _projectRepository.GetSingle(model.ID);
                    var item = Mapper.Map<ProjectViewModel>(project);

                    response = request.CreateResponse(HttpStatusCode.OK, new { success = true, item = item });
                }
                else
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });

                return response;
            });
        }

        [Authorize(Roles = "Division Chief, Deputy Executive Director, Executive Director, APD")]
        [Route("decline")]
        [HttpPost]
        public HttpResponseMessage DeclineProject(HttpRequestMessage request, ProjectViewModel model)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {
                    var currentUser = _userRepository.GetSingleByUsername(User.Identity.Name.Trim().ToLower());
                    var project = _projectRepository.GetSingle(model.ID);
                    if (project != null)
                    {
                        project.Decline(currentUser);
                        _unitOfWork.Commit();
                    }

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
