namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixresponse : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RequestAttachments", "TaskResponse_Id", "dbo.TaskResponses");
            DropIndex("dbo.RequestAttachments", new[] { "TaskResponse_Id" });
            DropColumn("dbo.RequestAttachments", "TaskResponse_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RequestAttachments", "TaskResponse_Id", c => c.Int());
            CreateIndex("dbo.RequestAttachments", "TaskResponse_Id");
            AddForeignKey("dbo.RequestAttachments", "TaskResponse_Id", "dbo.TaskResponses", "Id");
        }
    }
}
