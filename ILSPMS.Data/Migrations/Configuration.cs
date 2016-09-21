namespace ILSPMS.Data.Migrations
{
    using Common;
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ILSPMS.Data.ILSPMSContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ILSPMS.Data.ILSPMSContext context)
        {
            context.RoleSet.AddOrUpdate(GenerateRoles());
            context.UserSet.AddOrUpdate(CreateSystemAdmin());
            context.ProjectmovementTypeSet.AddOrUpdate(GenerateProjectMovementTypes());
            context.MilestoneSet.AddOrUpdate(GenerateMilestones());
            context.ApproverFlowByRoleSet.AddOrUpdate(GenerateApproverFlowByRoles());
        }

        private Role[] GenerateRoles()
        {
            var roles = new List<Role>();

            var enums = EnumerationHelper.GetEnumList(typeof(Enumerations.Role));
            foreach (var enumItem in enums)
            {
                roles.Add(new Role() { ID = enumItem.ID, Name = enumItem.Name });
            }

            return roles.ToArray();
        }

        private ProjectMovementType[] GenerateProjectMovementTypes()
        {
            var roles = new List<ProjectMovementType>();

            var enums = EnumerationHelper.GetEnumList(typeof(Enumerations.ProjectMovementType));
            foreach (var enumItem in enums)
            {
                roles.Add(new ProjectMovementType() { ID = enumItem.ID, Name = enumItem.Name });
            }

            return roles.ToArray();
        }

        private User CreateSystemAdmin()
        {
            var user = new User()
            {
                ID = 1,
                LastName = "Admin",
                FirstName = "Sys",
                Username = "admin",
                Email = "mvsvillanueva@gmail.com",
                HashedPassword = "9ni8N6zygK+PM92YBmEmd2qU1u9JcEnN3f/pqG4AVsU=", //test
                Salt = "WnUAwGIX44nFd/omrz/RoA==",
                RoleID = 1,
                IsLocked = false,
                Deleted = false,
                DateCreated = DateTime.Now
            };

            return user;
        }

        private Milestone[] GenerateMilestones()
        {
            var milestones = new List<Milestone>();

            milestones.Add(new Milestone()
            {
                ID = 1,
                Name = "Research Assignment",
                Order = 1
            });

            milestones.Add(new Milestone()
            {
                ID = 2,
                Name = "Concept Note Drafting",
                Order = 2
            });

            milestones.Add(new Milestone()
            {
                ID = 3,
                Name = "Data Gathering",
                Order = 3
            });

            milestones.Add(new Milestone()
            {
                ID = 4,
                Name = "Research Paper Drafting",
                Order = 4
            });

            milestones.Add(new Milestone()
            {
                ID = 5,
                Name = "Proof Reading",
                Order = 5
            });

            milestones.Add(new Milestone()
            {
                ID = 6,
                Name = "Layouting",
                Order = 6
            });

            milestones.Add(new Milestone()
            {
                ID = 7,
                Name = "Publishing",
                Order = 7
            });

            milestones.Add(new Milestone()
            {
                ID = 8,
                Name = "Published",
                Order = 8
            });

            return milestones.ToArray();
        }

        private ApproverFlowByRole[] GenerateApproverFlowByRoles()
        {
            var list = new List<ApproverFlowByRole>();

            //Concept Note Drafting
            list.Add(new ApproverFlowByRole() {
                ID = 1,
                MilestoneID = 2,
                ApproverRoleID = (int)Enumerations.Role.DivisionChief,
                NextApproverRoleID = (int)Enumerations.Role.DeputyExecDir,
                IsInitial = true
            });

            list.Add(new ApproverFlowByRole()
            {
                ID = 2,
                MilestoneID = 2,
                ApproverRoleID = (int)Enumerations.Role.DeputyExecDir,
                NextApproverRoleID = (int)Enumerations.Role.ExecDir,
                IsInitial = false
            });

            list.Add(new ApproverFlowByRole()
            {
                ID = 3,
                MilestoneID = 2,
                ApproverRoleID = (int)Enumerations.Role.ExecDir,
                NextApproverRoleID = null,
                IsInitial = false
            });

            //Research Paper Drafting
            list.Add(new ApproverFlowByRole()
            {
                ID = 4,
                MilestoneID = 4,
                ApproverRoleID = (int)Enumerations.Role.DivisionChief,
                NextApproverRoleID = (int)Enumerations.Role.DeputyExecDir,
                IsInitial = true
            });

            list.Add(new ApproverFlowByRole()
            {
                ID = 5,
                MilestoneID = 4,
                ApproverRoleID = (int)Enumerations.Role.DeputyExecDir,
                NextApproverRoleID = (int)Enumerations.Role.ExecDir,
                IsInitial = false
            });

            list.Add(new ApproverFlowByRole()
            {
                ID = 6,
                MilestoneID = 4,
                ApproverRoleID = (int)Enumerations.Role.ExecDir,
                NextApproverRoleID = null,
                IsInitial = false
            });

            //Proof reading
            list.Add(new ApproverFlowByRole()
            {
                ID = 7,
                MilestoneID = 5,
                ApproverRoleID = (int)Enumerations.Role.DivisionChief,
                NextApproverRoleID = (int)Enumerations.Role.DeputyExecDir,
                IsInitial = true
            });

            list.Add(new ApproverFlowByRole()
            {
                ID = 8,
                MilestoneID = 5,
                ApproverRoleID = (int)Enumerations.Role.DeputyExecDir,
                NextApproverRoleID = (int)Enumerations.Role.ExecDir,
                IsInitial = false
            });

            list.Add(new ApproverFlowByRole()
            {
                ID = 9,
                MilestoneID = 5,
                ApproverRoleID = (int)Enumerations.Role.ExecDir,
                NextApproverRoleID = null,
                IsInitial = false
            });

            //Layouting
            list.Add(new ApproverFlowByRole()
            {
                ID = 10,
                MilestoneID = 6,
                ApproverRoleID = (int)Enumerations.Role.DivisionChief,
                NextApproverRoleID = (int)Enumerations.Role.DeputyExecDir,
                IsInitial = true
            });

            list.Add(new ApproverFlowByRole()
            {
                ID = 11,
                MilestoneID = 6,
                ApproverRoleID = (int)Enumerations.Role.DeputyExecDir,
                NextApproverRoleID = (int)Enumerations.Role.ExecDir,
                IsInitial = false
            });

            list.Add(new ApproverFlowByRole()
            {
                ID = 12,
                MilestoneID = 6,
                ApproverRoleID = (int)Enumerations.Role.ExecDir,
                NextApproverRoleID = null,
                IsInitial = false
            });

            return list.ToArray();
        }
    }
}
