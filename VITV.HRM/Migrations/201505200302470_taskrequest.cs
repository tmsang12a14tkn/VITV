namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class taskrequest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaskRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        DeviceId = c.Int(),
                        RequestedEmployeeId = c.String(maxLength: 128),
                        ReceivedEmployeeId = c.String(maxLength: 128),
                        Piority = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.ReceivedEmployeeId)
                .ForeignKey("dbo.Employees", t => t.RequestedEmployeeId)
                .Index(t => t.RequestedEmployeeId)
                .Index(t => t.ReceivedEmployeeId);
            
            AddColumn("dbo.Tasks", "TaskRequestId", c => c.Int());
            CreateIndex("dbo.Tasks", "TaskRequestId");
            AddForeignKey("dbo.Tasks", "TaskRequestId", "dbo.TaskRequests", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "TaskRequestId", "dbo.TaskRequests");
            DropForeignKey("dbo.TaskRequests", "RequestedEmployeeId", "dbo.Employees");
            DropForeignKey("dbo.TaskRequests", "ReceivedEmployeeId", "dbo.Employees");
            DropIndex("dbo.TaskRequests", new[] { "ReceivedEmployeeId" });
            DropIndex("dbo.TaskRequests", new[] { "RequestedEmployeeId" });
            DropIndex("dbo.Tasks", new[] { "TaskRequestId" });
            DropColumn("dbo.Tasks", "TaskRequestId");
            DropTable("dbo.TaskRequests");
        }
    }
}
