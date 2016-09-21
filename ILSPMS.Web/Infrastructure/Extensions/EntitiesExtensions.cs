using ILSPMS.Common;
using ILSPMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ILSPMS.Web
{
    public static class EntitiesExtensions
    {
        public static void Initialize(this Project project)
        {
            if (project.ProjectManagerID != null)
            {
                var movement = new ProjectMovement()
                {
                    MilestoneID = 1,
                    ApproverRoleID = null,
                    DateApproved = null,
                    DateCreated = DateTime.Now,
                    DateSubmitted = null,
                    ProjectManagerID = (int)project.ProjectManagerID,
                    ProjectMovementTypeID = (int)Enumerations.ProjectMovementType.Init
                };
                project.ProjectMovements.Add(movement);
            }
        }
    }
}