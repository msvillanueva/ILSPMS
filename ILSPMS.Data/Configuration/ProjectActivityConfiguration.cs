using ILSPMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Data
{
    public class ProjectActivityConfiguration : EntityBaseConfiguration<ProjectActivity>
    {
        public ProjectActivityConfiguration()
        {
            Property(s => s.Activity)
                .IsRequired()
                .HasMaxLength(250);

            Property(s => s.ProjectID)
                .IsRequired();

            Property(s => s.UserID)
                .IsRequired();

            Property(s => s.BudgetUtilized)
                .IsRequired();

            Property(s => s.DateCreated)
                .IsRequired();

            Property(s => s.Deleted)
                .IsRequired();

            HasMany(s => s.ProjectActivityFiles)
                .WithRequired(s => s.ProjectActivity)
                .HasForeignKey(s => s.ProjectActivityID);
        }
    }
}
