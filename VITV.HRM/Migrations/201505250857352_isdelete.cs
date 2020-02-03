namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isdelete : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.TaskRequests", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.TaskResponses", "IsDeleted", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Projects", "Start", c => c.DateTime());
            AlterColumn("dbo.Projects", "End", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "End", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Projects", "Start", c => c.DateTime(nullable: false));
            DropColumn("dbo.TaskResponses", "IsDeleted");
            DropColumn("dbo.TaskRequests", "IsDeleted");
            DropColumn("dbo.Projects", "IsDeleted");
        }
    }
}
