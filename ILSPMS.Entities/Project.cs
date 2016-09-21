using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Entities
{
    public class Project : IEntityBase
    {
        public Project()
        {
            ProjectActivities = new List<ProjectActivity>();
            ProjectMovements = new List<ProjectMovement>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public int AddedByID { get; set; }
        public int DivisionID { get; set; }
        public int? ProjectManagerID { get; set; }
        public decimal Budget { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Deleted { get; set; }

        public virtual User AddedBy { get; set; }
        public virtual User ProjectManager { get; set; }
        public virtual Division Division { get; set; }
        public virtual ICollection<ProjectActivity> ProjectActivities { get; set; }
        public virtual ICollection<ProjectMovement> ProjectMovements { get; set; }
    }
}
