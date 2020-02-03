namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class projectrequestresponse : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Projects", "GroupId", "dbo.Groups");
            DropIndex("dbo.Projects", new[] { "GroupId" });
            CreateTable(
                "dbo.ProjectAttachments",
                c => new
                    {
                        ProjectId = c.Int(nullable: false),
                        AttachmentLink = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ProjectId, t.AttachmentLink })
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.TaskResponses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        TaskRequestId = c.Int(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TaskRequests", t => t.TaskRequestId, cascadeDelete: true)
                .Index(t => t.TaskRequestId);
            
            CreateTable(
                "dbo.ProjectEmployees",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        Employee_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.Employee_Id })
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.Employee_Id, cascadeDelete: true)
                .Index(t => t.Project_Id)
                .Index(t => t.Employee_Id);
            
            AddColumn("dbo.TaskRequests", "CreatedTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.TaskRequests", "ProjectId", c => c.Int());
            AddColumn("dbo.RequestAttachments", "TaskResponse_Id", c => c.Int());
            AddColumn("dbo.Projects", "Content", c => c.String());
            CreateIndex("dbo.TaskRequests", "ProjectId");
            CreateIndex("dbo.RequestAttachments", "TaskResponse_Id");
            AddForeignKey("dbo.TaskRequests", "ProjectId", "dbo.Projects", "Id");
            AddForeignKey("dbo.RequestAttachments", "TaskResponse_Id", "dbo.TaskResponses", "Id");
            DropColumn("dbo.Projects", "GroupId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "GroupId", c => c.Int(nullable: false));
            DropForeignKey("dbo.TaskResponses", "TaskRequestId", "dbo.TaskRequests");
            DropForeignKey("dbo.RequestAttachments", "TaskResponse_Id", "dbo.TaskResponses");
            DropForeignKey("dbo.TaskRequests", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ProjectEmployees", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.ProjectEmployees", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.ProjectAttachments", "ProjectId", "dbo.Projects");
            DropIndex("dbo.ProjectEmployees", new[] { "Employee_Id" });
            DropIndex("dbo.ProjectEmployees", new[] { "Project_Id" });
            DropIndex("dbo.TaskResponses", new[] { "TaskRequestId" });
            DropIndex("dbo.RequestAttachments", new[] { "TaskResponse_Id" });
            DropIndex("dbo.TaskRequests", new[] { "ProjectId" });
            DropIndex("dbo.ProjectAttachments", new[] { "ProjectId" });
            DropColumn("dbo.Projects", "Content");
            DropColumn("dbo.RequestAttachments", "TaskResponse_Id");
            DropColumn("dbo.TaskRequests", "ProjectId");
            DropColumn("dbo.TaskRequests", "CreatedTime");
            DropTable("dbo.ProjectEmployees");
            DropTable("dbo.TaskResponses");
            DropTable("dbo.ProjectAttachments");
            CreateIndex("dbo.Projects", "GroupId");
            AddForeignKey("dbo.Projects", "GroupId", "dbo.Groups", "Id", cascadeDelete: true);
        }
    }
}
