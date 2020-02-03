namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addequipment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Equipments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Status = c.String(),
                        Description = c.String(),
                        EmployeeId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .Index(t => t.EmployeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Equipments", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.Equipments", new[] { "EmployeeId" });
            DropTable("dbo.Equipments");
        }
    }
}
