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
    [RoutePrefix("api/movements")]
    public class ProjectMovementController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Project> _projectRepository;

        public ProjectMovementController(IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork,
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

                var projectMovements = new List<ProjectMilestoneMovementViewModel>();

                foreach(var milestone in project.ProjectMovements.Select(s => s.Milestone).Distinct().ToList())
                {
                    var milestoneMovement = new ProjectMilestoneMovementViewModel()
                    {
                        MilestoneID = milestone.ID,
                        MilestoneName = milestone.Name,
                        Movements = Mapper.Map<List<ProjectMovementViewModel>>(milestone.ProjectMovements.Where(s => s.ProjectID == project.ID))
                    };
                    projectMovements.Add(milestoneMovement);
                }

                response = request.CreateResponse(HttpStatusCode.OK, new { success = true, items = projectMovements, title = project.Name });

                return response;
            });
        }
    }
}
