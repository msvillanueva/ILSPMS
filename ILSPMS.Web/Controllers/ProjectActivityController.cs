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

        public ProjectActivityController(IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork,
            IEntityBaseRepository<Project> projectRepository
            ) : base (_errorsRepository, _unitOfWork)
        {
            _projectRepository = projectRepository;
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
                    foreach(var activity in milestone.ProjectActivities)
                    {
                        var projActivity = Mapper.Map<ProjectActivityViewModel>(activity);
                        var files = new List<ProjectActivityFileViewModel>();
                        foreach(var file in activity.ProjectActivityFiles)
                        {
                            files.Add(Mapper.Map<ProjectActivityFileViewModel>(file));
                        }
                        projActivity.Files.AddRange(files);
                    }
                    milestoneActivity.Activities.AddRange(activities);
                    projectActivities.Add(milestoneActivity);
                }

                response = request.CreateResponse(HttpStatusCode.OK, new { success = true, items = projectActivities, title = project.Name });

                return response;
            });
        }
    }
}
