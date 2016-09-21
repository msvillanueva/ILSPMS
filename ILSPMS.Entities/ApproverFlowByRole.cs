using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Entities
{
    public class ApproverFlowByRole : IEntityBase
    {
        public int ID { get; set; }
        public int MilestoneID { get; set; }
        public int ApproverRoleID { get; set; }
        public int? NextApproverRoleID { get; set; }
        public bool IsInitial { get; set; }

        public virtual Milestone Milestone { get; set; }
        public virtual Role ApproverRole { get; set; }
        public virtual Role NextApproverRole { get; set; }
    }
}
