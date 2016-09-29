using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ILSPMS.Web.Models
{
    public class ProjectActivityFileViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Filename { get; set; }
        public int ProjectActivityID { get; set; }
        public DateTime DateCreated { get; set; }

        public string Link
        {
            get { return $"files/ProjectFiles/{this.ID}/{this.Filename}"; }
        }
    }
}