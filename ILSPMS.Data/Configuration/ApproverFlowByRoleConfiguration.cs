using ILSPMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Data
{
    public class ApproverFlowByRoleConfiguration : EntityBaseConfiguration<ApproverFlowByRole>
    {
        public ApproverFlowByRoleConfiguration()
        {
            Property(s => s.MilestoneID)
                .IsRequired();

            Property(s => s.ApproverRoleID)
                .IsRequired();

            Property(s => s.NextApproverRoleID)
                .IsOptional();

            Property(s => s.IsInitial)
                .IsRequired();
        }
    }
}
