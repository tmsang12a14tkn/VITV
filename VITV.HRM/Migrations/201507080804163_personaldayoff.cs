namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class personaldayoff : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PersonalDayoffs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.String(maxLength: 128),
                        Title = c.String(),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        AllDay = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .Index(t => t.EmployeeId);
            
            DropColumn("dbo.PersonalJobs", "IsAbsent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PersonalJobs", "IsAbsent", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.PersonalDayoffs", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.PersonalDayoffs", new[] { "EmployeeId" });
            DropTable("dbo.PersonalDayoffs");
        }
    }
}
