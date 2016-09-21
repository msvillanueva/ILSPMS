using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Entities
{
    public class Milestone : IEntityBase
    {
        public Milestone()
        {
            ProjectMovements = new List<ProjectMovement>();
            ProjectActivities = new List<ProjectActivity>();
            ApproverFlowByRoles = new List<ApproverFlowByRole>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }

        public virtual ICollection<ProjectMovement> ProjectMovements { get; set; }
        public virtual ICollection<ProjectActivity> ProjectActivities { get; set; }
        public virtual ICollection<ApproverFlowByRole> ApproverFlowByRoles { get; set; }
    }
}
