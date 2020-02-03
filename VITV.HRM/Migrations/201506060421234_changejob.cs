namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changejob : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ResponseAttachments", "TaskResponseId", "dbo.TaskResponses");
            DropIndex("dbo.ResponseAttachments", new[] { "TaskResponseId" });
            AddColumn("dbo.Jobs", "EquipmentId", c => c.Int());
            AddColumn("dbo.Jobs", "StartTime", c => c.DateTime());
            AddColumn("dbo.Jobs", "EndTime", c => c.DateTime());
            AddColumn("dbo.RequestAttachments", "Job_Id", c => c.Int());
            AddColumn("dbo.TaskResponses", "Job_Id", c => c.Int());
            CreateIndex("dbo.Jobs", "EquipmentId");
            CreateIndex("dbo.RequestAttachments", "Job_Id");
            CreateIndex("dbo.TaskResponses", "Job_Id");
            AddForeignKey("dbo.RequestAttachments", "Job_Id", "dbo.Jobs", "Id");
            AddForeignKey("dbo.Jobs", "EquipmentId", "dbo.Equipments", "Id");
            AddForeignKey("dbo.TaskResponses", "Job_Id", "dbo.Jobs", "Id");
            DropTable("dbo.ResponseAttachments");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ResponseAttachments",
                c => new
                    {
                        TaskResponseId = c.Int(nullable: false),
                        AttachmentLink = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.TaskResponseId, t.AttachmentLink });
            
            DropForeignKey("dbo.TaskResponses", "Job_Id", "dbo.Jobs");
            DropForeignKey("dbo.Jobs", "EquipmentId", "dbo.Equipments");
            DropForeignKey("dbo.RequestAttachments", "Job_Id", "dbo.Jobs");
            DropIndex("dbo.TaskResponses", new[] { "Job_Id" });
            DropIndex("dbo.RequestAttachments", new[] { "Job_Id" });
            DropIndex("dbo.Jobs", new[] { "EquipmentId" });
            DropColumn("dbo.TaskResponses", "Job_Id");
            DropColumn("dbo.RequestAttachments", "Job_Id");
            DropColumn("dbo.Jobs", "EndTime");
            DropColumn("dbo.Jobs", "StartTime");
            DropColumn("dbo.Jobs", "EquipmentId");
            CreateIndex("dbo.ResponseAttachments", "TaskResponseId");
            AddForeignKey("dbo.ResponseAttachments", "TaskResponseId", "dbo.TaskResponses", "Id", cascadeDelete: true);
        }
    }
}
