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
    [RoutePrefix("api/divisions")]
    public class DivisionController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<Division> _divisionRepository;

        public DivisionController(IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork,
            IEntityBaseRepository<Division> divisionRepository
            ) : base(_errorsRepository, _unitOfWork)
        {
            _divisionRepository = divisionRepository;
        }

        [Route("")]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<Division> divisions = null;

                divisions = _divisionRepository
                    .FindBy(s => !s.Deleted)
                    .OrderBy(m => m.Name)
                    .ToList();

                var list = Mapper.Map<List<Division>, List<DivisionViewModel>>(divisions);

                response = request.CreateResponse(HttpStatusCode.OK, new { items = list } );

                return response;
            });
        }

        [Route("update")]
        [HttpPost]
        public HttpResponseMessage CreateEdit(HttpRequestMessage request, DivisionViewModel model)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {
                    if (model.ID > 0)
                    {
                        var division = _divisionRepository.GetSingle(model.ID);
                        if (division != null)
                        {
                            division.Name = model.Name;
                            _divisionRepository.Edit(division);
                            _unitOfWork.Commit();
                        }
                    }
                    else
                    {
                        if (_divisionRepository.FindBy(s => s.Name.Trim().ToLower() == model.Name.Trim().ToLower()).FirstOrDefault() != null)
                            return request.CreateResponse(HttpStatusCode.OK, new { success = false, message = "Division already exists" });

                        var division = new Division()
                        {
                            Name = model.Name.Trim(),
                            Deleted = false
                        };
                        _divisionRepository.Add(division);
                        _unitOfWork.Commit();
                        model.ID = division.ID;
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
        public HttpResponseMessage Archive(HttpRequestMessage request, DivisionViewModel model)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {
                    var divison = _divisionRepository.GetSingle(model.ID);
                    if (divison != null)
                    {
                        divison.Deleted = true;
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
