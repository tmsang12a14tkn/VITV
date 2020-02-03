namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addconference : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Conferences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConferenceEmployees",
                c => new
                    {
                        Conference_Id = c.Int(nullable: false),
                        Employee_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Conference_Id, t.Employee_Id })
                .ForeignKey("dbo.Conferences", t => t.Conference_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.Employee_Id, cascadeDelete: true)
                .Index(t => t.Conference_Id)
                .Index(t => t.Employee_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ConferenceEmployees", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.ConferenceEmployees", "Conference_Id", "dbo.Conferences");
            DropIndex("dbo.ConferenceEmployees", new[] { "Employee_Id" });
            DropIndex("dbo.ConferenceEmployees", new[] { "Conference_Id" });
            DropTable("dbo.ConferenceEmployees");
            DropTable("dbo.Conferences");
        }
    }
}
