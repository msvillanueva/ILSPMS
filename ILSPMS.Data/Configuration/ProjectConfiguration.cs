using ILSPMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Data
{
    public class ProjectConfiguration : EntityBaseConfiguration<Project>
    {
        public ProjectConfiguration()
        {
            Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(150);

            Property(s => s.AddedByID)
                .IsRequired();

            Property(s => s.ProjectManagerID)
                .IsOptional();

            Property(s => s.Budget)
                .IsRequired();

            Property(s => s.DateCreated)
                .IsRequired();

            Property(s => s.Deleted)
                .IsRequired();

            HasMany(s => s.ProjectActivities)
                .WithRequired(s => s.Project)
                .HasForeignKey(s => s.ProjectID);

            HasMany(s => s.ProjectMovements)
                .WithRequired(s => s.Project)
                .HasForeignKey(s => s.ProjectID);
        }
    }
}
