namespace ILSPMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Project_edit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "DivisionID", c => c.Int(nullable: false));
            CreateIndex("dbo.Project", "DivisionID");
            AddForeignKey("dbo.Project", "DivisionID", "dbo.Division", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Project", "DivisionID", "dbo.Division");
            DropIndex("dbo.Project", new[] { "DivisionID" });
            DropColumn("dbo.Project", "DivisionID");
        }
    }
}
