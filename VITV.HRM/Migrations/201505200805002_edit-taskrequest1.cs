namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edittaskrequest1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TaskRequests", "ReceivedEmployeeId", "dbo.Employees");
            DropIndex("dbo.TaskRequests", new[] { "ReceivedEmployeeId" });
            CreateTable(
                "dbo.EmployeeTaskRequests",
                c => new
                    {
                        Employee_Id = c.String(nullable: false, maxLength: 128),
                        TaskRequest_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Employee_Id, t.TaskRequest_Id })
                .ForeignKey("dbo.Employees", t => t.Employee_Id, cascadeDelete: true)
                .ForeignKey("dbo.TaskRequests", t => t.TaskRequest_Id, cascadeDelete: true)
                .Index(t => t.Employee_Id)
                .Index(t => t.TaskRequest_Id);
            
            DropColumn("dbo.TaskRequests", "ReceivedEmployeeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaskRequests", "ReceivedEmployeeId", c => c.String(maxLength: 128));
            DropForeignKey("dbo.EmployeeTaskRequests", "TaskRequest_Id", "dbo.TaskRequests");
            DropForeignKey("dbo.EmployeeTaskRequests", "Employee_Id", "dbo.Employees");
            DropIndex("dbo.EmployeeTaskRequests", new[] { "TaskRequest_Id" });
            DropIndex("dbo.EmployeeTaskRequests", new[] { "Employee_Id" });
            DropTable("dbo.EmployeeTaskRequests");
            CreateIndex("dbo.TaskRequests", "ReceivedEmployeeId");
            AddForeignKey("dbo.TaskRequests", "ReceivedEmployeeId", "dbo.Employees", "Id");
        }
    }
}
