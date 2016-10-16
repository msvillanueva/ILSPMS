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
    [Authorize(Roles = "Division Chief, Deputy Executive Director, Executive Director, APD")]
    [RoutePrefix("api/reportdata")]
    public class ReportController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Project> _projectRepository;
        private readonly IEntityBaseRepository<User> _userRepository;
        private readonly IEntityBaseRepository<Division> _divisionRepository;
        private readonly IEntityBaseRepository<Milestone> _milestoneRepository;

        public ReportController(IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork,
            IEntityBaseRepository<Project> projectRepository, IEntityBaseRepository<User> userRepository,
            IEntityBaseRepository<Division> divisionRepository, IEntityBaseRepository<Milestone> milestoneRepository
            ) : base (_errorsRepository, _unitOfWork)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _divisionRepository = divisionRepository;
            _milestoneRepository = milestoneRepository;
        }

        [Route("{divisionID?}/{year?}")]
        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request, int? divisionID = 0, int year = 0)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<Division> divisions = new List<Division>();
                var list = new List<DivisionReportViewModel>();

                var currentUser = _userRepository.GetSingleByUsername(User.Identity.Name.Trim().ToLower());

                if (currentUser.RoleID == (int)Enumerations.Role.DivisionChief)
                {
                    divisions.Add(currentUser.Division);
                }
                else
                {
                    divisions.AddRange(_divisionRepository
                        .FindBy(s => !s.Deleted && (divisionID == 0 || s.ID == divisionID)
                            && s.Projects.Any(sp => sp.DateCreated.Year == year && !sp.Deleted))
                        .OrderBy(s => s.Name).ToList());
                }

                foreach(var div in divisions)
                {
                    var divisionProjects = div.Projects
                        .Where(s => !s.Deleted && s.DateCreated.Year == year)
                        .ToList();

                    var item = new DivisionReportViewModel()
                    {
                        Division = Mapper.Map<DivisionViewModel>(div),
                        Projects = Mapper.Map<List<ProjectViewModel>>(divisionProjects)
                    };

                    item.TotalBudget = item.Projects.Sum(s => s.Budget);
                    item.TotalBudgetAllocated = item.Projects.Sum(s => s.BudgetUtilized);
                    item.BudgetUtilized = ((decimal)item.TotalBudgetAllocated / item.TotalBudget) * 100;
                    list.Add(item);
                }

                var topOrder = _milestoneRepository.GetAll().Max(s => s.Order);
                response = request.CreateResponse(HttpStatusCode.OK, new { items = list, ms = topOrder });

                return response;
            });
        }
    }
}
