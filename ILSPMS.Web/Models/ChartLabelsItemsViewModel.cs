using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ILSPMS.Web.Models
{
    public class ChartLabelsItemsViewModel
    {
        public string Name { get; set; }
        public List<string> Labels { get; set; }
        public List<int> Items { get; set; }
    }
}