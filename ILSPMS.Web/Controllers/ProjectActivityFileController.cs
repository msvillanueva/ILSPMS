using ILSPMS.Data;
using ILSPMS.Entities;
using ILSPMS.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ILSPMS.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/files")]
    public class ProjectActivityFileController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<ProjectActivityFile> _projectActivityFileRepository;

        public ProjectActivityFileController(IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork,
            IEntityBaseRepository<ProjectActivityFile> projectActivityFileRepository
            ) : base(_errorsRepository, _unitOfWork)
        {
            _projectActivityFileRepository = projectActivityFileRepository;
        }

        [Route("save")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, ProjectActivityFileViewModel model)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {
                    var activityFile = new ProjectActivityFile()
                    {
                        ProjectActivityID = model.ProjectActivityID,
                        Name = model.Name,
                        Filename = "",
                        DateCreated = DateTime.Now
                    };
                    _projectActivityFileRepository.Add(activityFile);
                    _unitOfWork.Commit();

                    response = request.CreateResponse(HttpStatusCode.OK, new { success = true, id = activityFile.ID });
                }
                else
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });

                return response;
            });
        }

        [MimeMultipart]
        [Route("upload")]
        public HttpResponseMessage Post(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var activityFile = _projectActivityFileRepository.GetSingle(id);
                if (activityFile == null)
                    response = request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid project activity info");
                else
                {
                    var uploadPath = HttpContext.Current.Server.MapPath($"~/files/activity/{activityFile.ID}");

                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    var httpRequest = HttpContext.Current.Request;
                    if (httpRequest.Files.Count > 0)
                    {

                        var postedFile = httpRequest.Files[0];
                        var filename = Guid.NewGuid().ToString().Replace("-", "");
                        var extension = Path.GetExtension(postedFile.FileName);
                        var filePath = uploadPath + $"\\{filename}{extension}";
                        postedFile.SaveAs(filePath);

                        activityFile.Filename = $"{filename}{extension}";
                        _projectActivityFileRepository.Edit(activityFile);

                        _unitOfWork.Commit();

                        response = request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                        response = request.CreateResponse(HttpStatusCode.OK, new { success = false, message = "Invalid file." });
                }

                return response;
            });
        }

        [Route("remove")]
        [HttpPost]
        public HttpResponseMessage Archive(HttpRequestMessage request, ProjectActivityFileViewModel model)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {
                    var file = _projectActivityFileRepository.GetSingle(model.ID);
                    if (file != null)
                    {
                        var uploadPath = HttpContext.Current.Server.MapPath($"~/files/activity/{model.ID}");

                        _projectActivityFileRepository.Delete(file);
                        _unitOfWork.Commit();

                        if (Directory.Exists(uploadPath))
                            Directory.Delete(uploadPath, true);
                    }

                    response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                }
                else
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });

                return response;
            });
        }
    }
}
