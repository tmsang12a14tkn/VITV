namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class employee_user : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationUsers", "EmployeeId", c => c.Int());
            CreateIndex("dbo.ApplicationUsers", "EmployeeId");
            AddForeignKey("dbo.ApplicationUsers", "EmployeeId", "dbo.Employees", "Id");
            DropColumn("dbo.Employees", "Phone");
            DropColumn("dbo.Employees", "Email");
            DropColumn("dbo.Employees", "Biography");
            DropColumn("dbo.Employees", "Introduction");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "Introduction", c => c.String());
            AddColumn("dbo.Employees", "Biography", c => c.String());
            AddColumn("dbo.Employees", "Email", c => c.String());
            AddColumn("dbo.Employees", "Phone", c => c.String());
            DropForeignKey("dbo.ApplicationUsers", "EmployeeId", "dbo.Employees");
            DropIndex("ApplicationUsers", new[] { "EmployeeId" });
            DropColumn("dbo.ApplicationUsers", "EmployeeId");
        }
    }
}
