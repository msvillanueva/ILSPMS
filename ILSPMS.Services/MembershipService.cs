using ILSPMS.Data;
using ILSPMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Services
{
    public class MembershipService : IMembershipService
    {
        #region Variables
        private readonly IEntityBaseRepository<User> _userRepository;
        private readonly IEntityBaseRepository<Role> _roleRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        public MembershipService(IEntityBaseRepository<User> userRepository, IEntityBaseRepository<Role> roleRepository,
        IEncryptionService encryptionService, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _encryptionService = encryptionService;
            _unitOfWork = unitOfWork;
        }

        #region IMembershipService Implementation

        public MembershipContext ValidateUser(string username, string password)
        {
            var membershipCtx = new MembershipContext();
            var user = _userRepository.GetSingleByUsername(username);

            if (user != null && isUserValid(user, password))
            {
                var userRoles = GetUserRole(username);
                membershipCtx.User = user;

                var identity = new GenericIdentity(username);
                membershipCtx.Principal = new GenericPrincipal(
                    identity,
                    userRoles.Select(x => x.Name).ToArray());
            }

            return membershipCtx;
        }

        public User CreateUser(User user, string password)
        {
            var existingUser = _userRepository.GetSingleByUsername(user.Username);

            if (existingUser != null)
            {
                throw new Exception("Username is already in use");
            }

            var passwordSalt = _encryptionService.CreateSalt();

            var newUser = user;
            newUser.Salt = passwordSalt;
            newUser.HashedPassword = _encryptionService.EncryptPassword(password, passwordSalt);
            newUser.DateCreated = DateTime.Now;

            _userRepository.Add(newUser);

            _unitOfWork.Commit();

            return newUser;
        }

        public User UpdateUserPassword(User user)
        {
            _userRepository.GetSingleByUsername(user.Username);

            var passwordSalt = _encryptionService.CreateSalt();

            var newUser = user;
            newUser.Salt = passwordSalt;
            newUser.HashedPassword = _encryptionService.EncryptPassword(user.Email, passwordSalt);

            newUser.DateCreated = DateTime.Now;

            _userRepository.Edit(newUser);
            _unitOfWork.Commit();

            return newUser;
        }

        public User GetUser(int userId)
        {
            return _userRepository.GetSingle(userId);
        }

        public List<Role> GetUserRole(string username)
        {
            List<Role> _result = new List<Role>();

            var existingUser = _userRepository.GetSingleByUsername(username);
            if (existingUser != null)
            {
                _result.Add(existingUser.Role);
            }

            return _result.Distinct().ToList();
        }

        public bool IsEmailUnique(string email)
        {
            return _userRepository.FindBy(s => s.Email.ToLower().Trim() == email.ToLower().Trim()).Count() == 0;
        }

        public bool IsEmailExist(string email)
        {
            return _userRepository.FindBy(s => s.Email.ToLower().Trim() == email.ToLower().Trim()).Count() > 0;
        }
        #endregion

        #region Helper methods

        private bool isPasswordValid(User user, string password)
        {
            return string.Equals(_encryptionService.EncryptPassword(password, user.Salt), user.HashedPassword);
        }

        private bool isUserValid(User user, string password)
        {
            if (isPasswordValid(user, password))
            {
                return !user.IsLocked;
            }

            return false;
        }
        #endregion
    }
}
