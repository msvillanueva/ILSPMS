using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ILSPMS.Web.Models
{
    public class ProjectViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int AddedByID { get; set; }
        public int DivisionID { get; set; }
        public int? ProjectManagerID { get; set; }
        public decimal Budget { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Deleted { get; set; }

        public string DivisionName { get; set; }
        public string AddedByName { get; set; }
        public string ProjectManager { get; set; }
        public string Year { get; set; }
        public string Milestone { get; set; }
        public string Activity { get; set; }
        public bool LockSubmit { get; set; }

        public double BudgetUtilized { get; set; }
        public int MilestoneOrder { get; set; }
    }
}