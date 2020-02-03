namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notification1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NotificationTypeId = c.Int(nullable: false),
                        FromId = c.String(maxLength: 128),
                        ProjectId = c.Int(),
                        RequestId = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.FromId)
                .ForeignKey("dbo.NotificationTypes", t => t.NotificationTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .ForeignKey("dbo.TaskRequests", t => t.RequestId)
                .Index(t => t.NotificationTypeId)
                .Index(t => t.FromId)
                .Index(t => t.ProjectId)
                .Index(t => t.RequestId);
            
            CreateTable(
                "dbo.NotificationTypes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Format = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserNotifications",
                c => new
                    {
                        EmployeeId = c.String(nullable: false, maxLength: 128),
                        NotificationId = c.Int(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.EmployeeId, t.NotificationId })
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.Notifications", t => t.NotificationId, cascadeDelete: true)
                .Index(t => t.EmployeeId)
                .Index(t => t.NotificationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserNotifications", "NotificationId", "dbo.Notifications");
            DropForeignKey("dbo.UserNotifications", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Notifications", "RequestId", "dbo.TaskRequests");
            DropForeignKey("dbo.Notifications", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Notifications", "NotificationTypeId", "dbo.NotificationTypes");
            DropForeignKey("dbo.Notifications", "FromId", "dbo.Employees");
            DropIndex("dbo.UserNotifications", new[] { "NotificationId" });
            DropIndex("dbo.UserNotifications", new[] { "EmployeeId" });
            DropIndex("dbo.Notifications", new[] { "RequestId" });
            DropIndex("dbo.Notifications", new[] { "ProjectId" });
            DropIndex("dbo.Notifications", new[] { "FromId" });
            DropIndex("dbo.Notifications", new[] { "NotificationTypeId" });
            DropTable("dbo.UserNotifications");
            DropTable("dbo.NotificationTypes");
            DropTable("dbo.Notifications");
        }
    }
}
