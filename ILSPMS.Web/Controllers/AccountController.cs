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
using System.Text;
using System.Web.Http;

namespace ILSPMS.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/account")]
    public class AccountController : ApiControllerBase
    {
        private readonly IMembershipService _membershipService;
        private readonly IEntityBaseRepository<User> _userRepository;
        private readonly IEncryptionService _encryptionService;

        public AccountController(IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork,
            IMembershipService membershipService, IEntityBaseRepository<User> userRepository,
            IEncryptionService encryptionService
            ) : base (_errorsRepository, _unitOfWork)
        {
            _membershipService = membershipService;
            _userRepository = userRepository;
            _encryptionService = encryptionService;
        }

        [Route("")]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var currentUser = _userRepository.GetSingleByUsername(User.Identity.Name.Trim().ToLower());
                var item = Mapper.Map<UserViewModel>(currentUser);
                response = request.CreateResponse(HttpStatusCode.OK, new { success = true, item = item });

                return response;
            });
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

        [Route("update")]
        [HttpPost]
        public HttpResponseMessage Update(HttpRequestMessage request, UserViewModel model)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {
                    var objUser = _userRepository.GetSingle(model.ID);
                    if (objUser != null)
                    {
                        objUser.FirstName = model.FirstName;
                        objUser.LastName = model.LastName;
                        objUser.Username = model.Username;
                        _userRepository.Edit(objUser);
                        _unitOfWork.Commit();

                        response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                    }
                    else
                        response = request.CreateResponse(HttpStatusCode.OK, new { success = false, message = "Invalid account info" });
                }
                else
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });

                return response;
            });
        }

        [Route("changepassword")]
        [HttpPost]
        public HttpResponseMessage UpdatePassword(HttpRequestMessage request, PasswordViewModel model)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {
                    byte[] data = Convert.FromBase64String(model.Current);
                    byte[] newPassword = Convert.FromBase64String(model.New);
                    var userCtx = _membershipService.ValidateUser(User.Identity.Name, Encoding.UTF8.GetString(data));

                    if (userCtx.User == null)
                        return request.CreateResponse(HttpStatusCode.OK, new { success = false, message = "Invalid password" });

                    if (model.New != model.Confirm)
                        return request.CreateResponse(HttpStatusCode.OK, new { success = false, message = "Passwords do not match" });

                    var user = userCtx.User;
                    user.Salt = _encryptionService.CreateSalt();
                    user.HashedPassword = _encryptionService.EncryptPassword(Encoding.UTF8.GetString(newPassword), user.Salt);
                    _userRepository.Edit(user);
                    _unitOfWork.Commit();

                    response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                }
                else
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });

                return response;
            });
        }

        [AllowAnonymous]
        [Route("forgot")]
        [HttpPost]
        public HttpResponseMessage SendForgotPassword(HttpRequestMessage request, UserViewModel user)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (ModelState.IsValid)
                {
                    var account = _userRepository.GetSingleByUsername(user.Email);
                    if (account != null)
                    {
                        var newPassword = _encryptionService.GenerateCode(6);
                        account.Salt = _encryptionService.CreateSalt();
                        account.HashedPassword = _encryptionService.EncryptPassword(newPassword, account.Salt);
                        _unitOfWork.Commit();

                        var email = new EmailSender()
                        {
                            RecipientName = $"{account.FirstName} {account.LastName}",
                            To = new List<string>() { account.Email }
                        };
                        email.SendForgotPasswordAsync(newPassword);

                        response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                    }
                    else
                    {
                        response = request.CreateResponse(HttpStatusCode.OK, new { success = false, message = user.Email + " is not yet registered." });
                    }

                }
                else
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });

                return response;
            });
        }
    }
}
