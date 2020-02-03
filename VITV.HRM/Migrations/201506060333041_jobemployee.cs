namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jobemployee : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobEmployees",
                c => new
                    {
                        Job_Id = c.Int(nullable: false),
                        Employee_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Job_Id, t.Employee_Id })
                .ForeignKey("dbo.Jobs", t => t.Job_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.Employee_Id, cascadeDelete: true)
                .Index(t => t.Job_Id)
                .Index(t => t.Employee_Id);
            
            AddColumn("dbo.Jobs", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobEmployees", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.JobEmployees", "Job_Id", "dbo.Jobs");
            DropIndex("dbo.JobEmployees", new[] { "Employee_Id" });
            DropIndex("dbo.JobEmployees", new[] { "Job_Id" });
            DropColumn("dbo.Jobs", "Description");
            DropTable("dbo.JobEmployees");
        }
    }
}
