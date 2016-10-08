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
    [RoutePrefix("api/dashboard")]
    public class DashboardController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Project> _projectRepository;
        private readonly IEntityBaseRepository<User> _userRepository;
        private readonly IEntityBaseRepository<Division> _divisionRepository;
        private readonly IEntityBaseRepository<Milestone> _milestoneRepository;

        public DashboardController(IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork,
            IEntityBaseRepository<Project> projectRepository, IEntityBaseRepository<User> userRepository,
            IEntityBaseRepository<Division> divisionRepository, IEntityBaseRepository<Milestone> milestoneRepository
            ) : base(_errorsRepository, _unitOfWork)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _divisionRepository = divisionRepository;
            _milestoneRepository = milestoneRepository;
        }

        [Route("")]
        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<Project> objProjects = null;

                var forApprovals = 0;

                var year = DateTime.Now.Year;
                var currentUser = _userRepository.GetSingleByUsername(User.Identity.Name.Trim().ToLower());


                forApprovals = _projectRepository
                .FindBy(s => !s.Deleted && s.ProjectMovements.OrderByDescending(pm => pm.ID).FirstOrDefault() != null
                    && !s.ProjectMovements.OrderByDescending(pm => pm.ID).FirstOrDefault().IsApproved
                    && (s.ProjectMovements.OrderByDescending(pm => pm.ID).FirstOrDefault().ProjectMovementTypeID == (int)Enumerations.ProjectMovementType.ForApproval
                        || s.ProjectMovements.OrderByDescending(pm => pm.ID).FirstOrDefault().ProjectMovementTypeID == (int)Enumerations.ProjectMovementType.NextApproval)
                    && s.ProjectMovements.OrderByDescending(pm => pm.ID).FirstOrDefault().ApproverRoleID == currentUser.RoleID
                    && (s.DivisionID == currentUser.DivisionID || currentUser.RoleID == (int)Enumerations.Role.DeputyExecDir
                        || currentUser.RoleID == (int)Enumerations.Role.ExecDir)
                    )
                .ToList().Count();


                if (currentUser.RoleID == (int)Enumerations.Role.PM)
                {
                    objProjects = _projectRepository
                        .FindBy(s => !s.Deleted && s.ProjectManagerID == currentUser.ID
                                && (year == 0 || s.DateCreated.Year == year)
                                )
                            .OrderBy(m => m.Name)
                            .ToList();
                }
                else if (currentUser.RoleID == (int)Enumerations.Role.DivisionChief)
                {
                    objProjects = _projectRepository
                        .FindBy(s => !s.Deleted && (s.DivisionID == currentUser.DivisionID || s.ProjectManagerID == currentUser.ID)
                            && (year == 0 || s.DateCreated.Year == year)
                            )
                        .OrderBy(m => m.Name)
                        .ToList();
                }
                else
                {
                    objProjects = _projectRepository
                        .FindBy(s => !s.Deleted
                            && (year == 0 || s.DateCreated.Year == year)
                            )
                        .OrderBy(m => m.Name)
                        .ToList();
                }

                var topMs = _milestoneRepository.GetAll().Max(s => s.Order);
                var list = Mapper.Map<List<Project>, List<ProjectViewModel>>(objProjects);

                var approvalRateLabels = new List<string>() { "In-progress", "Completed" };
                var approvalRateValues = new List<int>()
                {
                    list.Where(s => s.MilestoneOrder < topMs).Count(),
                    list.Where(s => s.MilestoneOrder == topMs && s.Activity == "Closed").Count()
                };
                var dougnutData = new ChartLabelsItemsViewModel()
                {
                    Name = "Closed projects",
                    Labels = approvalRateLabels,
                    Items = approvalRateValues
                };

                var listMovements = new List<string>();
                var movements = new List<ProjectMovement>();
                var activities = new List<ProjectActivity>();
                foreach(var proj in objProjects)
                {
                    movements.AddRange(proj.ProjectMovements);
                    activities.AddRange(proj.ProjectActivities);
                }

                foreach(var movement in movements.OrderByDescending(s => s.ID).Take(15).ToList())
                {
                    var mvm = Mapper.Map<ProjectMovementViewModel>(movement);
                    var approver = mvm.ApproverName != "" ? $"-{mvm.ApproverName}" : "";
                    var pm = mvm.ApproverName == "" ? $"-{mvm.ProjectManagerName}" : "";
                    listMovements.Add($"{mvm.DateCreated.ToString("dd-MMM-yyy")}: [{mvm.ProjectName}] {mvm.ProjectMovementTypeName} {approver} {pm}");
                }

                response = request.CreateResponse(HttpStatusCode.OK, new
                {
                    projects = list,
                    approval = forApprovals,
                    closeProject = dougnutData,
                    movements = listMovements,
                    totalMovements = movements.Count(),
                    totalActivities = activities.Count()
                });

                return response;
            });
        }


    }
}
