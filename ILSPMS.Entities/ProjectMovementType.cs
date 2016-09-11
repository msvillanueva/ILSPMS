using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Entities
{
    public class ProjectMovementType : IEntityBase
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ProjectMovement> ProjectMovements { get; set; }
    }
}
