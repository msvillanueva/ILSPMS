using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Entities
{
    public class ProjectActivityFile : IEntityBase
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Filename { get; set; }
        public int ProjectActivityID { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual ProjectActivity ProjectActivity { get; set; }
    }
}
