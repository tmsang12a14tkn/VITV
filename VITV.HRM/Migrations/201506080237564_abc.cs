namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class abc : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobResponses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        JobId = c.Int(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        EmployeeId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .ForeignKey("dbo.Jobs", t => t.JobId, cascadeDelete: true)
                .Index(t => t.JobId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.ResponseAttachments",
                c => new
                    {
                        JobResponseId = c.Int(nullable: false),
                        AttachmentLink = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.JobResponseId, t.AttachmentLink })
                .ForeignKey("dbo.JobResponses", t => t.JobResponseId, cascadeDelete: true)
                .Index(t => t.JobResponseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobResponses", "JobId", "dbo.Jobs");
            DropForeignKey("dbo.JobResponses", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.ResponseAttachments", "JobResponseId", "dbo.JobResponses");
            DropIndex("dbo.ResponseAttachments", new[] { "JobResponseId" });
            DropIndex("dbo.JobResponses", new[] { "EmployeeId" });
            DropIndex("dbo.JobResponses", new[] { "JobId" });
            DropTable("dbo.ResponseAttachments");
            DropTable("dbo.JobResponses");
        }
    }
}
