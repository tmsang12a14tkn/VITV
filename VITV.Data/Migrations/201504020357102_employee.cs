namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class employee : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Reporters", newName: "Employees");
            RenameTable(name: "dbo.ReporterVideos", newName: "EmployeeVideos");
            RenameTable(name: "dbo.RoleReporters", newName: "RoleEmployees");
            RenameColumn(table: "dbo.EmployeeVideos", name: "Reporter_Id", newName: "Employee_Id");
            RenameColumn(table: "dbo.RoleEmployees", name: "Reporter_Id", newName: "Employee_Id");
            RenameIndex(table: "dbo.RoleEmployees", name: "IX_Reporter_Id", newName: "IX_Employee_Id");
            RenameIndex(table: "dbo.EmployeeVideos", name: "IX_Reporter_Id", newName: "IX_Employee_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.EmployeeVideos", name: "IX_Employee_Id", newName: "IX_Reporter_Id");
            RenameIndex(table: "dbo.RoleEmployees", name: "IX_Employee_Id", newName: "IX_Reporter_Id");
            RenameColumn(table: "dbo.RoleEmployees", name: "Employee_Id", newName: "Reporter_Id");
            RenameColumn(table: "dbo.EmployeeVideos", name: "Employee_Id", newName: "Reporter_Id");
            RenameTable(name: "dbo.RoleEmployees", newName: "RoleReporters");
            RenameTable(name: "dbo.EmployeeVideos", newName: "ReporterVideos");
            RenameTable(name: "dbo.Employees", newName: "Reporters");
        }
    }
}
