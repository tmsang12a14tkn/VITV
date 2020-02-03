namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixjob : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TaskResponses", "Job_Id", "dbo.Jobs");
            DropIndex("dbo.TaskResponses", new[] { "Job_Id" });
            DropColumn("dbo.TaskResponses", "Job_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaskResponses", "Job_Id", c => c.Int());
            CreateIndex("dbo.TaskResponses", "Job_Id");
            AddForeignKey("dbo.TaskResponses", "Job_Id", "dbo.Jobs", "Id");
        }
    }
}
