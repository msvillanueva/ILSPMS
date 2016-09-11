using ILSPMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Services
{
    public interface IMembershipService
    {
        MembershipContext ValidateUser(string username, string password);
        User CreateUser(User user, string password);
        User GetUser(int userId);
        List<Role> GetUserRole(string username);
        bool IsEmailUnique(string email);

        bool IsEmailExist(string email);
    }
}
