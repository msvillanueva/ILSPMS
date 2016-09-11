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
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/roles")]
    public class RoleController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Role> _roleRepository;

        public RoleController(IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork,
            IEntityBaseRepository<Role> roleRepository
            ) : base (_errorsRepository, _unitOfWork)
        {
            _roleRepository = roleRepository;
        }

        [Route("")]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<Role> roles = null;

                roles = _roleRepository
                    .GetAll()
                    .OrderBy(s => s.ID)
                    .ToList();

                var list = Mapper.Map<List<Role>, List<RoleViewModel>>(roles);
                response = request.CreateResponse(HttpStatusCode.OK, new { items = list });

                return response;
            });
        }
    }
}
