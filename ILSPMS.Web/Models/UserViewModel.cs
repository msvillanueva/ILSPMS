using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ILSPMS.Web.Models
{
    public class UserViewModel
    {
        public int ID { get; set; }
        public int? DivisionID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public int RoleID { get; set; }
        public bool IsLocked { get; set; }
        public bool Deleted { get; set; }
        public DateTime DateCreated { get; set; }

        public string RoleName { get; set; }
        public string FullName { get; set; }
        public string DivisionName { get; set; }
    }
}