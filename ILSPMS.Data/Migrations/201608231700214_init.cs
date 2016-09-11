namespace ILSPMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApproverFlowByRole",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ApproverID = c.Int(nullable: false),
                        NextApproverID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Role", t => t.ApproverID, cascadeDelete: true)
                .ForeignKey("dbo.Role", t => t.NextApproverID)
                .Index(t => t.ApproverID)
                .Index(t => t.NextApproverID);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DivisionID = c.Int(),
                        LastName = c.String(nullable: false, maxLength: 100),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        Username = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false, maxLength: 200),
                        HashedPassword = c.String(nullable: false, maxLength: 255),
                        Salt = c.String(nullable: false, maxLength: 100),
                        RoleID = c.Int(nullable: false),
                        IsLocked = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Division", t => t.DivisionID)
                .ForeignKey("dbo.Role", t => t.RoleID, cascadeDelete: true)
                .Index(t => t.DivisionID)
                .Index(t => t.RoleID);
            
            CreateTable(
                "dbo.ProjectMovement",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectID = c.Int(nullable: false),
                        ProjectManagerID = c.Int(nullable: false),
                        MilestoneID = c.Int(nullable: false),
                        ProjectMovementTypeID = c.Int(nullable: false),
                        IsSubmitted = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        ApproverID = c.Int(),
                        DateSubmitted = c.DateTime(),
                        DateApproved = c.DateTime(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Project", t => t.ProjectID, cascadeDelete: true)
                .ForeignKey("dbo.Milestone", t => t.MilestoneID, cascadeDelete: true)
                .ForeignKey("dbo.ProjectMovementType", t => t.ProjectMovementTypeID, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.ApproverID)
                .ForeignKey("dbo.User", t => t.ProjectManagerID, cascadeDelete: true)
                .Index(t => t.ProjectID)
                .Index(t => t.ProjectManagerID)
                .Index(t => t.MilestoneID)
                .Index(t => t.ProjectMovementTypeID)
                .Index(t => t.ApproverID);
            
            CreateTable(
                "dbo.Milestone",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Order = c.Int(nullable: false),
                        ApproverRoleID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Role", t => t.ApproverRoleID)
                .Index(t => t.ApproverRoleID);
            
            CreateTable(
                "dbo.ProjectActivity",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Activity = c.String(nullable: false, maxLength: 250),
                        ProjectID = c.Int(nullable: false),
                        MilestoneID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        BudgetUtilized = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Project", t => t.ProjectID, cascadeDelete: true)
                .ForeignKey("dbo.Milestone", t => t.MilestoneID, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
                .Index(t => t.ProjectID)
                .Index(t => t.MilestoneID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Project",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        AddedByID = c.Int(nullable: false),
                        ProjectManagerID = c.Int(),
                        Budget = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.AddedByID)
                .ForeignKey("dbo.User", t => t.ProjectManagerID)
                .Index(t => t.AddedByID)
                .Index(t => t.ProjectManagerID);
            
            CreateTable(
                "dbo.ProjectActivityFile",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Filename = c.String(nullable: false, maxLength: 200),
                        ProjectActivityID = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ProjectActivity", t => t.ProjectActivityID, cascadeDelete: true)
                .Index(t => t.ProjectActivityID);
            
            CreateTable(
                "dbo.ProjectMovementType",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Division",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Error",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Message = c.String(nullable: false),
                        StackTrace = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User", "RoleID", "dbo.Role");
            DropForeignKey("dbo.Project", "ProjectManagerID", "dbo.User");
            DropForeignKey("dbo.ProjectMovement", "ProjectManagerID", "dbo.User");
            DropForeignKey("dbo.ProjectActivity", "UserID", "dbo.User");
            DropForeignKey("dbo.User", "DivisionID", "dbo.Division");
            DropForeignKey("dbo.Project", "AddedByID", "dbo.User");
            DropForeignKey("dbo.ProjectMovement", "ApproverID", "dbo.User");
            DropForeignKey("dbo.ProjectMovement", "ProjectMovementTypeID", "dbo.ProjectMovementType");
            DropForeignKey("dbo.ProjectMovement", "MilestoneID", "dbo.Milestone");
            DropForeignKey("dbo.ProjectActivity", "MilestoneID", "dbo.Milestone");
            DropForeignKey("dbo.ProjectActivityFile", "ProjectActivityID", "dbo.ProjectActivity");
            DropForeignKey("dbo.ProjectMovement", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.ProjectActivity", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.Milestone", "ApproverRoleID", "dbo.Role");
            DropForeignKey("dbo.ApproverFlowByRole", "NextApproverID", "dbo.Role");
            DropForeignKey("dbo.ApproverFlowByRole", "ApproverID", "dbo.Role");
            DropIndex("dbo.ProjectActivityFile", new[] { "ProjectActivityID" });
            DropIndex("dbo.Project", new[] { "ProjectManagerID" });
            DropIndex("dbo.Project", new[] { "AddedByID" });
            DropIndex("dbo.ProjectActivity", new[] { "UserID" });
            DropIndex("dbo.ProjectActivity", new[] { "MilestoneID" });
            DropIndex("dbo.ProjectActivity", new[] { "ProjectID" });
            DropIndex("dbo.Milestone", new[] { "ApproverRoleID" });
            DropIndex("dbo.ProjectMovement", new[] { "ApproverID" });
            DropIndex("dbo.ProjectMovement", new[] { "ProjectMovementTypeID" });
            DropIndex("dbo.ProjectMovement", new[] { "MilestoneID" });
            DropIndex("dbo.ProjectMovement", new[] { "ProjectManagerID" });
            DropIndex("dbo.ProjectMovement", new[] { "ProjectID" });
            DropIndex("dbo.User", new[] { "RoleID" });
            DropIndex("dbo.User", new[] { "DivisionID" });
            DropIndex("dbo.ApproverFlowByRole", new[] { "NextApproverID" });
            DropIndex("dbo.ApproverFlowByRole", new[] { "ApproverID" });
            DropTable("dbo.Error");
            DropTable("dbo.Division");
            DropTable("dbo.ProjectMovementType");
            DropTable("dbo.ProjectActivityFile");
            DropTable("dbo.Project");
            DropTable("dbo.ProjectActivity");
            DropTable("dbo.Milestone");
            DropTable("dbo.ProjectMovement");
            DropTable("dbo.User");
            DropTable("dbo.Role");
            DropTable("dbo.ApproverFlowByRole");
        }
    }
}
