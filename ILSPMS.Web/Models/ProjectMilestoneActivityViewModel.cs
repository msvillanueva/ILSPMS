using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ILSPMS.Web.Models
{
    public class ProjectMilestoneActivityViewModel
    {
        public ProjectMilestoneActivityViewModel()
        {
            Activities = new List<ProjectActivityViewModel>();
        }

        public int MilestoneID { get; set; }
        public string MilestoneName { get; set; }

        public List<ProjectActivityViewModel> Activities { get; set; }
    }
}