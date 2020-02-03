namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reponseemployee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskResponses", "EmployeeId", c => c.String(maxLength: 128));
            CreateIndex("dbo.TaskResponses", "EmployeeId");
            AddForeignKey("dbo.TaskResponses", "EmployeeId", "dbo.Employees", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskResponses", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.TaskResponses", new[] { "EmployeeId" });
            DropColumn("dbo.TaskResponses", "EmployeeId");
        }
    }
}
