using ILSPMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Data
{
    public class RoleConfiguration : EntityBaseConfiguration<Role>
    {
        public RoleConfiguration()
        {
            Property(s => s.ID)
               .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            HasMany(s => s.Users)
                .WithRequired(s => s.Role)
                .HasForeignKey(s => s.RoleID);

            HasMany(s => s.ApproverFlowByRoles)
                .WithRequired(s => s.ApproverRole)
                .HasForeignKey(s => s.ApproverRoleID);

            HasMany(s => s.NextApproverFlowByRoles)
                .WithOptional(s => s.NextApproverRole)
                .HasForeignKey(s => s.NextApproverRoleID)
                .WillCascadeOnDelete(false);

            HasMany(s => s.ApprovedProjectMovements)
                .WithOptional(s => s.ApproverRole)
                .HasForeignKey(s => s.ApproverRoleID);
        }
    }
}
