namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jobattachmentjobid : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobAttachments", "Job_Id", "dbo.Jobs");
            DropIndex("dbo.JobAttachments", new[] { "Job_Id" });
            RenameColumn(table: "dbo.JobAttachments", name: "Job_Id", newName: "JobId");
            DropPrimaryKey("dbo.JobAttachments");
            AlterColumn("dbo.JobAttachments", "JobId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.JobAttachments", new[] { "JobId", "AttachmentLink" });
            CreateIndex("dbo.JobAttachments", "JobId");
            AddForeignKey("dbo.JobAttachments", "JobId", "dbo.Jobs", "Id", cascadeDelete: true);
            DropColumn("dbo.JobAttachments", "TaskRequestId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobAttachments", "TaskRequestId", c => c.Int(nullable: false));
            DropForeignKey("dbo.JobAttachments", "JobId", "dbo.Jobs");
            DropIndex("dbo.JobAttachments", new[] { "JobId" });
            DropPrimaryKey("dbo.JobAttachments");
            AlterColumn("dbo.JobAttachments", "JobId", c => c.Int());
            AddPrimaryKey("dbo.JobAttachments", new[] { "TaskRequestId", "AttachmentLink" });
            RenameColumn(table: "dbo.JobAttachments", name: "JobId", newName: "Job_Id");
            CreateIndex("dbo.JobAttachments", "Job_Id");
            AddForeignKey("dbo.JobAttachments", "Job_Id", "dbo.Jobs", "Id");
        }
    }
}
