using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Entities
{
    public class ProjectActivity : IEntityBase
    {
        public int ID { get; set; }
        public string Activity { get; set; }
        public int ProjectID { get; set; }
        public int MilestoneID { get; set; }
        public int UserID { get; set; }
        public decimal BudgetUtilized { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Deleted { get; set; }

        public virtual Project Project { get; set; }
        public virtual Milestone Milestone { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<ProjectActivityFile> ProjectActivityFiles { get; set; }
    }
}
