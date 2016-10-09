using ILSPMS.Common;
using ILSPMS.Data;
using ILSPMS.Entities;
using ILSPMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ILSPMS.Web.Controllers
{
    [Authorize(Roles ="Admin")]
    [RoutePrefix("api/admindashboard")]
    public class AdminDashboardController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Project> _projectRepository;
        private readonly IEntityBaseRepository<User> _userRepository;
        private readonly IEntityBaseRepository<ProjectMovement> _projectMovementRepository;

        public AdminDashboardController(IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork,
            IEntityBaseRepository<Project> projectRepository, IEntityBaseRepository<User> userRepository,
            IEntityBaseRepository<ProjectMovement> projectMovementRepository
            ) : base (_errorsRepository, _unitOfWork)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _projectMovementRepository = projectMovementRepository;
        }

        [Route("")]
        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var year = DateTime.Now.Year;

                var projects = _projectRepository.FindBy(s => !s.Deleted && s.DateCreated.Year == year).Count();
                var allProjects = _projectRepository.FindBy(s => !s.Deleted).Count();
                var users = _userRepository.FindBy(s => !s.Deleted).Count();
                var months = DateTimeFormatInfo.CurrentInfo.MonthNames.Where(s => s != "").ToList();
                var movements = _projectMovementRepository.FindBy(s => s.DateCreated.Year == year).ToList()
                    .GroupBy(s => s.DateCreated.ToString("MMMM"))
                    .Select(gp => new {
                        Month = gp.Key,
                        Count = gp.Count()
                    })
                    .ToList();

                var movementData = new List<int>();
                foreach(var month in months)
                {
                    var monthMovements = movements.Where(s => s.Month == month).FirstOrDefault();
                    movementData.Add(monthMovements != null ? monthMovements.Count : 0);
                }

                var movementsData = new ChartLabelsItemsViewModel()
                {
                    Name = "Movements data",
                    Labels = months,
                    Items = movementData
                };

                response = request.CreateResponse(HttpStatusCode.OK, new
                {
                    projects = projects,
                    allProjects = allProjects,
                    users = users,
                    movementsData = movementsData
                });

                return response;
            });
        }
    }
}
