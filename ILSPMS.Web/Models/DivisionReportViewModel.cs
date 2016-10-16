using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ILSPMS.Web.Models
{
    public class DivisionReportViewModel
    {
        public DivisionViewModel Division { get; set; }
        public List<ProjectViewModel> Projects { get; set; }
        public Decimal TotalBudget { get; set; }
        public double TotalBudgetAllocated { get; set; }
        public Decimal BudgetUtilized { get; set; }
    }
}