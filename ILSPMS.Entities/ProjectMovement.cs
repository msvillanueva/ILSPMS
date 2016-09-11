using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Entities
{
    public class ProjectMovement : IEntityBase
    {
        public int ID { get; set; }
        public int ProjectID { get; set; }
        public int ProjectManagerID { get; set; }
        public int MilestoneID { get; set; }
        public int ProjectMovementTypeID { get; set; }
        public bool IsSubmitted { get; set; }
        public bool IsApproved { get; set; }
        public int? ApproverID { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? DateApproved { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Project Project { get; set; }
        public virtual Milestone Milestone { get; set; }
        public virtual User Approver { get; set; }
        public virtual User ProjectManager { get; set; }
        public virtual ProjectMovementType ProjectMovementType { get; set; }
    }
}
