using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ILSPMS.Web.Models
{
    public class ProjectMilestoneMovementViewModel
    {
        public ProjectMilestoneMovementViewModel()
        {
            Movements = new List<ProjectMovementViewModel>();
        }

        public int MilestoneID { get; set; }
        public string MilestoneName { get; set; }
        
        public List<ProjectMovementViewModel> Movements { get; set; }
    }
}