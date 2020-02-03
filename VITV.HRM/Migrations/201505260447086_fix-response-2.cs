namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixresponse2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ResponseAttachments",
                c => new
                    {
                        TaskResponseId = c.Int(nullable: false),
                        AttachmentLink = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.TaskResponseId, t.AttachmentLink })
                .ForeignKey("dbo.TaskResponses", t => t.TaskResponseId, cascadeDelete: true)
                .Index(t => t.TaskResponseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ResponseAttachments", "TaskResponseId", "dbo.TaskResponses");
            DropIndex("dbo.ResponseAttachments", new[] { "TaskResponseId" });
            DropTable("dbo.ResponseAttachments");
        }
    }
}
