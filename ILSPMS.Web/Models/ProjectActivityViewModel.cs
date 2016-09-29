using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ILSPMS.Web.Models
{
    public class ProjectActivityViewModel
    {
        public ProjectActivityViewModel()
        {
            Files = new List<ProjectActivityFileViewModel>();
        }

        public int ID { get; set; }
        public string Activity { get; set; }
        public int ProjectID { get; set; }
        public int MilestoneID { get; set; }
        public int UserID { get; set; }
        public decimal BudgetUtilized { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Deleted { get; set; }

        public string ProjectManagerName { get; set; }
        public List<ProjectActivityFileViewModel> Files { get; set; }
    }
}