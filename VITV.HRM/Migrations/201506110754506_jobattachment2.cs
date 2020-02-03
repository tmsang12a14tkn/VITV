namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jobattachment2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobAttachments",
                c => new
                    {
                        TaskRequestId = c.Int(nullable: false),
                        AttachmentLink = c.String(nullable: false, maxLength: 128),
                        Job_Id = c.Int(),
                    })
                .PrimaryKey(t => new { t.TaskRequestId, t.AttachmentLink })
                .ForeignKey("dbo.Jobs", t => t.Job_Id)
                .Index(t => t.Job_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobAttachments", "Job_Id", "dbo.Jobs");
            DropIndex("dbo.JobAttachments", new[] { "Job_Id" });
            DropTable("dbo.JobAttachments");
        }
    }
}
