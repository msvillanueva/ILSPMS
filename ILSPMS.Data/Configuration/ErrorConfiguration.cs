using ILSPMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Data
{
    public class ErrorConfiguration : EntityBaseConfiguration<Error>
    {
        public ErrorConfiguration()
        {
            Property(s => s.Message)
                .IsRequired();

            Property(s => s.StackTrace)
                .IsRequired();

            Property(s => s.DateCreated)
                .IsRequired();

        }
    }
}
