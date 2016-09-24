using ILSPMS.Common;
using ILSPMS.Data;
using ILSPMS.Data.Repositories;
using ILSPMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityBaseRepository<Milestone> _milestoneRepository;

        public ProjectService(IUnitOfWork unitOfWork, IEntityBaseRepository<Milestone> milestoneRepository)
        {
            _unitOfWork = unitOfWork;
            _milestoneRepository = milestoneRepository;
        }

        public void Submit(Project project)
        {
            var isReadyNextMilestone = false;
            var latestMovement = project.ProjectMovements.OrderByDescending(s => s.DateCreated)
                .FirstOrDefault();
            var latestMileStone = latestMovement.Milestone;

            if (latestMileStone.ApproverFlowByRoles.Count() > 0)
            {
                if (latestMovement.ApproverRoleID == null) // initial approver
                {
                    var initialApproverFlow = latestMileStone.ApproverFlowByRoles.FirstOrDefault(s => s.IsInitial);
                    if (initialApproverFlow != null)
                    {
                        var forApprovalMovement = new ProjectMovement()
                        {
                            ProjectMovementTypeID = (int)Enumerations.ProjectMovementType.ForApproval,
                            ApproverRoleID = initialApproverFlow.ApproverRoleID,
                            ApproverUserID = null,
                            DateApproved = null,
                            DateCreated = DateTime.Now,
                            DateSubmitted = null,
                            MilestoneID = latestMileStone.ID,
                            IsSubmitted = false,
                            ProjectID = project.ID,
                            IsApproved = false,
                            ProjectManagerID = (int)project.ProjectManagerID
                        };
                        project.ProjectMovements.Add(forApprovalMovement);
                    }
                }
                else //next approver if any
                {
                    var approverFlow = latestMileStone.ApproverFlowByRoles
                        .FirstOrDefault(s => s.ApproverRoleID == latestMovement.ApproverRoleID);

                    if (approverFlow != null && approverFlow.NextApproverRoleID != null) //next approver
                    {
                        var forNextApprovalMovement = new ProjectMovement()
                        {
                            ProjectMovementTypeID = (int)Enumerations.ProjectMovementType.NextApproval,
                            ApproverRoleID = approverFlow.NextApproverRoleID,
                            ApproverUserID = null,
                            DateApproved = null,
                            DateCreated = DateTime.Now,
                            DateSubmitted = null,
                            MilestoneID = latestMileStone.ID,
                            IsSubmitted = false,
                            ProjectID = project.ID,
                            IsApproved = false,
                            ProjectManagerID = (int)project.ProjectManagerID
                        };
                        project.ProjectMovements.Add(forNextApprovalMovement);
                    }
                    else
                    {
                        isReadyNextMilestone = true;
                    }
                }
            }
            else
            {
                isReadyNextMilestone = true;
            }

            if (isReadyNextMilestone) //has no next approver
            {
                //close milestone
                var submitMovement = new ProjectMovement()
                {
                    ProjectMovementTypeID = (int)Enumerations.ProjectMovementType.Submitted,
                    ApproverRoleID = null,
                    ApproverUserID = null,
                    DateApproved = null,
                    DateCreated = DateTime.Now,
                    DateSubmitted = DateTime.Now,
                    MilestoneID = latestMileStone.ID,
                    IsSubmitted = true,
                    ProjectID = project.ID,
                    IsApproved = true,
                    ProjectManagerID = (int)project.ProjectManagerID
                };
                project.ProjectMovements.Add(submitMovement);
                
                //next milestone
                var nextMilestone = _milestoneRepository.FindBy(s => s.Order == latestMileStone.Order + 1).FirstOrDefault();
                if (nextMilestone != null)
                {
                    var newMovement = new ProjectMovement()
                    {
                        ProjectMovementTypeID = (int)Enumerations.ProjectMovementType.Init,
                        ApproverRoleID = null,
                        ApproverUserID = null,
                        DateApproved = null,
                        DateCreated = DateTime.Now,
                        DateSubmitted = null,
                        MilestoneID = nextMilestone.ID,
                        IsSubmitted = false,
                        ProjectID = project.ID,
                        IsApproved = false,
                        ProjectManagerID = (int)project.ProjectManagerID
                    };
                    project.ProjectMovements.Add(newMovement);
                }
            }

            _unitOfWork.Commit();
        }
    }
}
