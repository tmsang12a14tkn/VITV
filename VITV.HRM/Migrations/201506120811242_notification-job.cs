namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notificationjob : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "JobId", c => c.Int());
            CreateIndex("dbo.Notifications", "JobId");
            AddForeignKey("dbo.Notifications", "JobId", "dbo.Jobs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "JobId", "dbo.Jobs");
            DropIndex("dbo.Notifications", new[] { "JobId" });
            DropColumn("dbo.Notifications", "JobId");
        }
    }
}
