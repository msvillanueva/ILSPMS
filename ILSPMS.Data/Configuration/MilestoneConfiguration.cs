using ILSPMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Data
{
    public class MilestoneConfiguration: EntityBaseConfiguration<Milestone>
    {
        public MilestoneConfiguration()
        {
            Property(s => s.ID)
               .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            Property(s => s.Order)
                .IsRequired();

            HasMany(s => s.ProjectMovements)
                .WithRequired(s => s.Milestone)
                .HasForeignKey(s => s.MilestoneID);

            HasMany(s => s.ProjectActivities)
                .WithRequired(s => s.Milestone)
                .HasForeignKey(s => s.MilestoneID);

            HasMany(s => s.ApproverFlowByRoles)
                .WithRequired(s => s.Milestone)
                .HasForeignKey(s => s.MilestoneID);
        }
    }
}
