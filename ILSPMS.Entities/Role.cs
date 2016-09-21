using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Entities
{
    public class Role : IEntityBase
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<ApproverFlowByRole> ApproverFlowByRoles { get; set; }
        public virtual ICollection<ApproverFlowByRole> NextApproverFlowByRoles { get; set; }
        public virtual ICollection<ProjectMovement> ApprovedProjectMovements { get; set; }
    }
}
