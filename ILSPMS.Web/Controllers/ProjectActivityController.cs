using AutoMapper;
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
    [RoutePrefix("api/activities")]
    public class ProjectActivityController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Project> _projectRepository;
        private readonly IEntityBaseRepository<ProjectActivity> _projectActivityRepository;
        private readonly IEntityBaseRepository<User> _userRepository;

        public ProjectActivityController(IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork,
            IEntityBaseRepository<Project> projectRepository, IEntityBaseRepository<ProjectActivity> projectActivityRepository,
            IEntityBaseRepository<User> userRepository
            ) : base (_errorsRepository, _unitOfWork)
        {
            _projectRepository = projectRepository;
            _projectActivityRepository = projectActivityRepository;
            _userRepository = userRepository;
        }

        [Route("{id?}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var project = _projectRepository.GetSingle(id);
                if (project == null)
                    return request.CreateResponse(HttpStatusCode.OK, new { success = false, message = "Invalid project info" });

                var projectActivities = new List<ProjectMilestoneActivityViewModel>();

                foreach (var milestone in project.ProjectMovements.Select(s => s.Milestone).Distinct().ToList())
                {
                    var milestoneActivity = new ProjectMilestoneActivityViewModel()
                    {
                        MilestoneID = milestone.ID,
                        MilestoneName = milestone.Name
                    };

                    var activities = new List<ProjectActivityViewModel>();
                    foreach(var activity in milestone.ProjectActivities.Where(s => s.ProjectID == project.ID && !s.Deleted))
                    {
                        var projActivity = Mapper.Map<ProjectActivityViewModel>(activity);
                        var files = new List<ProjectActivityFileViewModel>();
                        foreach(var file in activity.ProjectActivityFiles)
                        {
                            files.Add(Mapper.Map<ProjectActivityFileViewModel>(file));
                        }
                        projActivity.Files.AddRange(files);
                        activities.Add(projActivity);
                    }
                    milestoneActivity.Activities.AddRange(activities);
                    projectActivities.Add(milestoneActivity);
                }

                var vmProject = Mapper.Map<ProjectViewModel>(project);

                response = request.CreateResponse(HttpStatusCode.OK, new { success = true, items = projectActivities, project = vmProject });

                return response;
            });
        }

        [Route("update")]
        [HttpPost]
        public HttpResponseMessage CreateEdit(HttpRequestMessage request, ProjectActivityViewModel model)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {
                    var currentUser = _userRepository.GetSingleByUsername(User.Identity.Name.Trim().ToLower());

                    if (model.ID > 0)
                    {
                        var activity = _projectActivityRepository.GetSingle(model.ID);
                        if (activity != null)
                        {
                            activity.Activity = model.Activity;
                            activity.BudgetUtilized = model.BudgetUtilized;
                            _projectActivityRepository.Edit(activity);
                            _unitOfWork.Commit();
                        }
                    }
                    else
                    {
                        var newActivity = new ProjectActivity()
                        {
                            Activity = model.Activity.Trim(),
                            BudgetUtilized = model.BudgetUtilized,
                            Deleted = false,
                            DateCreated = DateTime.Now,
                            MilestoneID = model.MilestoneID,
                            ProjectID = model.ProjectID,
                            UserID = currentUser.ID
                        };
                        _projectActivityRepository.Add(newActivity);
                        _unitOfWork.Commit();
                        model.ID = newActivity.ID;
                    }

                    response = request.CreateResponse(HttpStatusCode.OK, new { success = true, item = model });
                }
                else
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });

                return response;
            });
        }

        [Route("remove")]
        [HttpPost]
        public HttpResponseMessage Archive(HttpRequestMessage request, ProjectActivityViewModel model)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {
                    var obj = _projectActivityRepository.GetSingle(model.ID);
                    if (obj != null)
                    {
                        obj.Deleted = true;
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
