using ILSPMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Data
{
    public class ProjectActivityFileConfiguration : EntityBaseConfiguration<ProjectActivityFile>
    {
        public ProjectActivityFileConfiguration()
        {
            Property(s => s.ProjectActivityID)
                .IsRequired();

            Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            Property(s => s.Filename)
                .IsRequired()
                .HasMaxLength(200);

            Property(s => s.DateCreated)
                .IsRequired();
        }
    }
}
