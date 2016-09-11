using ILSPMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Data
{
    public class ProjectMovementTypeConfiguration : EntityBaseConfiguration<ProjectMovementType>
    {
        public ProjectMovementTypeConfiguration()
        {
            Property(s => s.ID)
               .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            HasMany(s => s.ProjectMovements)
                .WithRequired(s => s.ProjectMovementType)
                .HasForeignKey(s => s.ProjectMovementTypeID);
        }
    }
}
