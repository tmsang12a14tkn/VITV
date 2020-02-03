namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TaskEmployees", "Task_Id", "dbo.Tasks");
            DropForeignKey("dbo.TaskEmployees", "Employee_Id", "dbo.Employees");
            DropIndex("dbo.TaskEmployees", new[] { "Task_Id" });
            DropIndex("dbo.TaskEmployees", new[] { "Employee_Id" });
            AlterColumn("dbo.Tasks", "EmployeeId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Tasks", "EmployeeId");
            AddForeignKey("dbo.Tasks", "EmployeeId", "dbo.Employees", "Id");
            DropTable("dbo.TaskEmployees");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TaskEmployees",
                c => new
                    {
                        Task_Id = c.Int(nullable: false),
                        Employee_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Task_Id, t.Employee_Id });
            
            DropForeignKey("dbo.Tasks", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.Tasks", new[] { "EmployeeId" });
            AlterColumn("dbo.Tasks", "EmployeeId", c => c.String());
            CreateIndex("dbo.TaskEmployees", "Employee_Id");
            CreateIndex("dbo.TaskEmployees", "Task_Id");
            AddForeignKey("dbo.TaskEmployees", "Employee_Id", "dbo.Employees", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TaskEmployees", "Task_Id", "dbo.Tasks", "Id", cascadeDelete: true);
        }
    }
}
