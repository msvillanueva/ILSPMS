using ILSPMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Data
{
    public class DivisionConfiguration : EntityBaseConfiguration<Division>
    {
        public DivisionConfiguration()
        {
            Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            Property(s => s.Deleted)
                .IsRequired();

            HasMany(s => s.Users)
                .WithOptional(s => s.Division)
                .HasForeignKey(s => s.DivisionID);
        }
    }
}
