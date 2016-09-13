using AutoMapper;
using ILSPMS.Common;
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
    [Authorize]
    [RoutePrefix("api/users")]
    public class UserController : ApiControllerBase
    {
        private readonly IEntityBaseRepository<User> _userRepository;
        private readonly IEncryptionService _encryptionService;

        public UserController(IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork,
            IEntityBaseRepository<User> userRepository, IEncryptionService encryptionService
            ) : base(_errorsRepository, _unitOfWork)
        {
            _userRepository = userRepository;
            _encryptionService = encryptionService;
        }

        [Route("{filter?}")]
        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request, string filter = null)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<User> users = null;
                List<User> selected = null;
                int totalCount = 0;

                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.Trim().ToLower();
                    selected = _userRepository
                        .FindBy(s => !s.Deleted && (s.FirstName.ToLower().Contains(filter.ToLower().Trim()) || s.LastName.ToLower().Contains(filter.ToLower().Trim())
                            || s.Email.ToLower().Contains(filter.ToLower().Trim()) || s.Username.ToLower().Contains(filter.ToLower().Trim())))
                        .ToList();
                }
                else
                {
                    selected = _userRepository
                        .FindBy(s => !s.Deleted)
                        .ToList();
                }

                var currentUser = _userRepository.GetSingleByUsername(User.Identity.Name.Trim().ToLower());

                users = selected
                    .Where(s => s.ID != currentUser.ID)
                    .OrderBy(m => m.FirstName)
                    .ToList();

                totalCount = users.Count();

                IEnumerable<UserViewModel> usersVM = Mapper.Map<IEnumerable<User>, IEnumerable<UserViewModel>>(users);

                response = request.CreateResponse(HttpStatusCode.OK, new { items = usersVM });

                return response;
            });
        }

        [Authorize(Roles = "Admin, Division Officer")]
        [Route("pms")]
        [HttpGet]
        public HttpResponseMessage GetPMs(HttpRequestMessage request, string filter = null)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<User> users = null;
                List<User> selected = null;
                int totalCount = 0;

                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.Trim().ToLower();
                    selected = _userRepository
                        .FindBy(s => !s.Deleted && (s.FirstName.ToLower().Contains(filter.ToLower().Trim()) || s.LastName.ToLower().Contains(filter.ToLower().Trim())
                            || s.Email.ToLower().Contains(filter.ToLower().Trim()) || s.Username.ToLower().Contains(filter.ToLower().Trim())))
                        .ToList();
                }
                else
                {
                    selected = _userRepository
                        .FindBy(s => !s.Deleted)
                        .ToList();
                }

                var currentUser = _userRepository.GetSingleByUsername(User.Identity.Name.Trim().ToLower());

                users = selected
                    .Where(s => s.ID != currentUser.ID)
                    .OrderBy(m => m.FirstName)
                    .ToList();

                totalCount = users.Count();

                IEnumerable<UserViewModel> usersVM = Mapper.Map<IEnumerable<User>, IEnumerable<UserViewModel>>(users);

                response = request.CreateResponse(HttpStatusCode.OK, new { items = usersVM });

                return response;
            });
        }

        [Authorize(Roles = "Admin")]
        [Route("new")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, UserViewModel model)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {
                    if (_userRepository.FindBy(s => s.Email.Trim().ToLower() == model.Email.ToLower().Trim()).FirstOrDefault() == null)
                    {
                        var salt = _encryptionService.CreateSalt();
                        var pass = _encryptionService.GenerateCode(6);
                        var newUser = new User()
                        {
                            DivisionID = model.DivisionID,
                            LastName = model.LastName,
                            FirstName = model.FirstName,
                            Username = "",
                            Email = model.Email,
                            HashedPassword = _encryptionService.EncryptPassword(pass, salt),
                            Salt = salt,
                            RoleID = model.RoleID,
                            IsLocked = false,
                            Deleted = false,
                            DateCreated = DateTime.Now
                        };

                        _userRepository.Add(newUser);
                        _unitOfWork.Commit();

                        var email = new EmailSender()
                        {
                            RecipientName = $"{model.FirstName} {model.LastName}",
                            To = new List<string>() { model.Email }
                        };
                        //email.SendAcceptRegistrationAsync(pass);
                        var objUser = _userRepository.GetSingle(newUser.ID);
                        var userVM = Mapper.Map<UserViewModel>(objUser);

                        response = request.CreateResponse(HttpStatusCode.OK, new { success = true, item = userVM });
                    }
                    else
                    {
                        response = request.CreateResponse(HttpStatusCode.OK, new { success = false, message = "The email is already in use." });
                    }
                }
                else
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });

                return response;
            });
        }

        [Authorize(Roles = "Admin")]
        [Route("edit")]
        [HttpPost]
        public HttpResponseMessage Update(HttpRequestMessage request, UserViewModel model)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {
                    if (_userRepository.FindBy(s => s.Email.Trim().ToLower() == model.Email.ToLower().Trim() && s.ID != model.ID).FirstOrDefault() == null)
                    {
                        var objUser = _userRepository.GetSingle(model.ID);
                        if (objUser != null)
                        {
                            objUser.FirstName = model.FirstName;
                            objUser.LastName = model.LastName;
                            objUser.Email = model.Email;
                            objUser.IsLocked = model.IsLocked;
                            objUser.DivisionID = model.DivisionID;
                            objUser.RoleID = model.RoleID;
                        }

                        _unitOfWork.Commit();
                        var user = _userRepository.GetSingle(objUser.ID);
                        model = Mapper.Map<UserViewModel>(user);
                        response = request.CreateResponse(HttpStatusCode.OK, new { success = true, item = model });
                    }
                    else
                    {
                        response = request.CreateResponse(HttpStatusCode.OK, new { success = false, message = "The email is already in use." });
                    }
                }
                else
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });

                return response;
            });
        }

        [Authorize(Roles = "Admin")]
        [Route("remove")]
        [HttpPost]
        public HttpResponseMessage Archive(HttpRequestMessage request, UserViewModel model)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {
                    var objUser = _userRepository.GetSingle(model.ID);
                    if (objUser != null)
                    {
                        objUser.Deleted = true;
                    }
                    _unitOfWork.Commit();
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                }
                else
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });

                return response;
            });
        }

        [Authorize(Roles = "Admin")]
        [Route("lockunlock")]
        [HttpPost]
        public HttpResponseMessage ToggleLock(HttpRequestMessage request, UserViewModel model)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {
                    var objUser = _userRepository.GetSingle(model.ID);
                    if (objUser != null)
                    {
                        objUser.IsLocked = model.IsLocked;
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
