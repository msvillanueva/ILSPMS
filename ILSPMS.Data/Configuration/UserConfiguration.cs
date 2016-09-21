using ILSPMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Data
{
    public class UserConfiguration : EntityBaseConfiguration<User>
    {
        public UserConfiguration()
        {
            Property(s => s.DivisionID)
                .IsOptional();

            Property(s => s.LastName)
                .IsRequired()
                .HasMaxLength(100);

            Property(s => s.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            Property(s => s.Username)
                .IsRequired()
                .HasMaxLength(20);

            Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(200);

            Property(s => s.HashedPassword)
                .IsRequired()
                .HasMaxLength(255);

            Property(s => s.Salt)
                .IsRequired()
                .HasMaxLength(100);

            Property(s => s.RoleID)
                .IsRequired();

            Property(s => s.IsLocked)
                .IsRequired();

            Property(s => s.DateCreated)
                .IsRequired();

            Property(s => s.Deleted)
                .IsRequired();

            HasMany(s => s.Projects)
               .WithOptional(s => s.ProjectManager)
               .HasForeignKey(s => s.ProjectManagerID)
               .WillCascadeOnDelete(false);

            HasMany(s => s.CreatedProjects)
                .WithRequired(s => s.AddedBy)
                .HasForeignKey(s => s.AddedByID)
                .WillCascadeOnDelete(false);

            HasMany(s => s.ProjectActivities)
                .WithRequired(s => s.User)
                .HasForeignKey(s => s.UserID);

            HasMany(s => s.ProjectMovements)
                .WithRequired(s => s.ProjectManager)
                .HasForeignKey(s => s.ProjectManagerID);

            HasMany(s => s.ApprovedProjectMovements)
                .WithOptional(s => s.ApproverUser)
                .HasForeignKey(s => s.ApproverUserID);
        }
    }
}
