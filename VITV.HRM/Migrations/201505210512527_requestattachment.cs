namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requestattachment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RequestAttachments",
                c => new
                    {
                        TaskRequestId = c.Int(nullable: false),
                        AttachmentLink = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.TaskRequestId, t.AttachmentLink })
                .ForeignKey("dbo.TaskRequests", t => t.TaskRequestId, cascadeDelete: true)
                .Index(t => t.TaskRequestId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequestAttachments", "TaskRequestId", "dbo.TaskRequests");
            DropIndex("dbo.RequestAttachments", new[] { "TaskRequestId" });
            DropTable("dbo.RequestAttachments");
        }
    }
}
