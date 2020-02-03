namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jobattachment1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RequestAttachments", "TaskRequestId", "dbo.TaskRequests");
            DropForeignKey("dbo.RequestAttachments", "Job_Id", "dbo.Jobs");
            DropIndex("dbo.RequestAttachments", new[] { "TaskRequestId" });
            DropIndex("dbo.RequestAttachments", new[] { "Job_Id" });
            DropTable("dbo.RequestAttachments");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RequestAttachments",
                c => new
                    {
                        TaskRequestId = c.Int(nullable: false),
                        AttachmentLink = c.String(nullable: false, maxLength: 128),
                        Job_Id = c.Int(),
                    })
                .PrimaryKey(t => new { t.TaskRequestId, t.AttachmentLink });
            
            CreateIndex("dbo.RequestAttachments", "Job_Id");
            CreateIndex("dbo.RequestAttachments", "TaskRequestId");
            AddForeignKey("dbo.RequestAttachments", "Job_Id", "dbo.Jobs", "Id");
            AddForeignKey("dbo.RequestAttachments", "TaskRequestId", "dbo.TaskRequests", "Id", cascadeDelete: true);
        }
    }
}
