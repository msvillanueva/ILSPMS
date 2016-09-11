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
    [Authorize(Roles = "Admin, User")]
    [RoutePrefix("api/account")]
    public class AccountController : ApiControllerBase
    {
        private readonly IMembershipService _membershipService;
        private readonly IEntityBaseRepository<User> _userRepository;

        public AccountController(IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork,
            IMembershipService membershipService, IEntityBaseRepository<User> userRepository
            ) : base (_errorsRepository, _unitOfWork)
        {
            _membershipService = membershipService;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [Route("authenticate")]
        [HttpPost]
        public HttpResponseMessage Login(HttpRequestMessage request, LoginViewModel user)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {
                    MembershipContext _userContext = _membershipService.ValidateUser(user.Username, user.Password);

                    if (_userContext.User != null)
                    {
                        response = request.CreateResponse(HttpStatusCode.OK, new { success = true, role = _userContext.User.RoleID, id = _userContext.User.ID });
                    }
                    else
                    {
                        response = request.CreateResponse(HttpStatusCode.OK, new { success = false });
                    }
                }
                else
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });

                return response;
            });
        }
    }
}
