using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ILSPMS.Web.Models
{
    public class ProjectMovementViewModel
    {
        public int ID { get; set; }
        public int ProjectID { get; set; }
        public int ProjectManagerID { get; set; }
        public int MilestoneID { get; set; }
        public int ProjectMovementTypeID { get; set; }
        public bool IsSubmitted { get; set; }
        public bool IsApproved { get; set; }
        public int? ApproverRoleID { get; set; }
        public int? ApproverUserID { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? DateApproved { get; set; }
        public DateTime DateCreated { get; set; }

        public string ProjectMovementTypeName { get; set; }
        public string ProjectManagerName { get; set; }
        public string ApproverName { get; set; }
        public string ApproverRoleName { get; set; }
        public string ProjectName { get; set; }
    }
}