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
                var newMovement = new ProjectMovement()
                {
                    ApproverRoleID = latestMovement.ApproverRoleID,
                    ApproverUserID = user.ID,
                    DateApproved = DateTime.Now,
                    DateSubmitted = latestMovement.DateSubmitted,
                    ApproverRole = latestMovement.ApproverRole,
                    ApproverUser = user,
                    DateCreated = DateTime.Now,
                    IsApproved = true,
                    IsSubmitted = latestMovement.IsSubmitted,
                    Milestone = latestMovement.Milestone,
                    MilestoneID = latestMovement.MilestoneID,
                    Project = latestMovement.Project,
                    ProjectID = latestMovement.ProjectID,
                    ProjectManager = latestMovement.ProjectManager,
                    ProjectManagerID = latestMovement.ProjectManagerID,
                    ProjectMovementTypeID = (int)Enumerations.ProjectMovementType.Approved
                };

                project.ProjectMovements.Add(newMovement);
            }
        }
    }
}