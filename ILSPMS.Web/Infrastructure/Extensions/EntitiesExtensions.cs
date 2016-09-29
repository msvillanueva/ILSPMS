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

        public static void Approve(this Project project, User user)
        {
            var latestMovement = project.ProjectMovements.OrderByDescending(s => s.DateCreated).FirstOrDefault();
            if (latestMovement != null && latestMovement.ApproverRoleID != null && !latestMovement.IsApproved)
            {
                var copiedLatestMovement = latestMovement;
                copiedLatestMovement.ApproverUser = user;
                copiedLatestMovement.ApproverUserID = user.ID;
                copiedLatestMovement.IsApproved = true;
                copiedLatestMovement.DateApproved = DateTime.Now;
                copiedLatestMovement.DateCreated = DateTime.Now;

                project.ProjectMovements.Add(copiedLatestMovement);
            }
        }
    }
}