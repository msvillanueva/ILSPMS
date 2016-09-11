using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Entities
{
    public class Milestone : IEntityBase
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public int? ApproverRoleID { get; set; }

        public virtual Role ApproverRole { get; set; }
        public virtual ICollection<ProjectMovement> ProjectMovements { get; set; }
        public virtual ICollection<ProjectActivity> ProjectActivities { get; set; }
    }
}
