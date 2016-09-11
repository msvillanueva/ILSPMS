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
        public int ApproverID { get; set; }
        public int? NextApproverID { get; set; }

        public virtual Role Approver { get; set; }
        public virtual Role NextApprover { get; set; }
    }
}
