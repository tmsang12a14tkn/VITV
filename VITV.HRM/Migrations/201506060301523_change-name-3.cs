namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changename3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PersonalJobs", "TaskRequestId", "dbo.TaskRequests");
            DropIndex("dbo.PersonalJobs", new[] { "TaskRequestId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.PersonalJobs", "TaskRequestId");
            AddForeignKey("dbo.PersonalJobs", "TaskRequestId", "dbo.TaskRequests", "Id");
        }
    }
}
