using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Entities
{
    public class User : IEntityBase
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

        public virtual Division Division { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Project> CreatedProjects { get; set; }
        public virtual ICollection<ProjectActivity> ProjectActivities { get; set; }
        public virtual ICollection<ProjectMovement> ProjectMovements { get; set; }
        public virtual ICollection<ProjectMovement> ApprovedProjectMovements { get; set; }
    }
}
