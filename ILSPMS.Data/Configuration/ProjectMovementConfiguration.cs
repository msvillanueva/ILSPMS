using ILSPMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Data
{
    public class ProjectMovementConfiguration : EntityBaseConfiguration<ProjectMovement>
    {
        public ProjectMovementConfiguration()
        {
            Property(s => s.ProjectID)
                .IsRequired();

            Property(s => s.ProjectManagerID)
                .IsRequired();

            Property(s => s.MilestoneID)
                .IsRequired();

            Property(s => s.ProjectMovementTypeID)
                .IsRequired();

            Property(s => s.IsSubmitted)
                .IsRequired();

            Property(s => s.IsApproved)
                .IsRequired();

            Property(s => s.ApproverRoleID)
                .IsOptional();

            Property(s => s.ApproverUserID)
                .IsOptional();

            Property(s => s.DateSubmitted)
                .IsOptional();

            Property(s => s.DateApproved)
                .IsOptional();

            Property(s => s.DateCreated)
                .IsRequired();
        }
    }
}
