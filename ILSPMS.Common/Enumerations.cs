using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Common
{
    public class Enumerations
    {
        public enum Role
        {
            [Description("Admin")]
            Admin = 1,

            [Description("Project Manager")]
            PM = 2,

            [Description("Division Chief")]
            DivisionChief = 3,

            [Description("Deputy Executive Director")]
            DeputyExecDir = 4,

            [Description("Executive Director")]
            ExecDir = 5
        }

        public enum ProjectMovementType
        {
            [Description("Initialize")]
            Init = 1,

            [Description("For Approval")]
            ForApproval = 2,

            [Description("Next Approval")]
            NextApproval = 3,

            [Description("Rejected")]
            Rejected = 4,

            [Description("Approved")]
            Approved = 5,

            [Description("Submitted")]
            Submitted = 6,

            [Description("Closed")]
            Closed = 7
        }
    }
}
