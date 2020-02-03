namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class department : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        DepartmentRoleId = c.Int(nullable: false),
                        DepartmentLocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DepartmentLocations", t => t.DepartmentLocationId, cascadeDelete: true)
                .ForeignKey("dbo.DepartmentRoles", t => t.DepartmentRoleId, cascadeDelete: true)
                .Index(t => t.DepartmentRoleId)
                .Index(t => t.DepartmentLocationId);
            
            CreateTable(
                "dbo.DepartmentLocations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DepartmentRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmployeeInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Phone = c.String(),
                        Email = c.String(),
                        Biography = c.String(),
                        Introduction = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Id)
                .Index(t => t.Id);
            
            AddColumn("dbo.Employees", "DepartmentId", c => c.Int());
            CreateIndex("dbo.Employees", "DepartmentId");
            AddForeignKey("dbo.Employees", "DepartmentId", "dbo.Departments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmployeeInfoes", "Id", "dbo.Employees");
            DropForeignKey("dbo.Employees", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Departments", "DepartmentRoleId", "dbo.DepartmentRoles");
            DropForeignKey("dbo.Departments", "DepartmentLocationId", "dbo.DepartmentLocations");
            DropIndex("EmployeeInfoes", new[] { "Id" });
            DropIndex("Departments", new[] { "DepartmentLocationId" });
            DropIndex("Departments", new[] { "DepartmentRoleId" });
            DropIndex("Employees", new[] { "DepartmentId" });
            DropColumn("dbo.Employees", "DepartmentId");
            DropTable("dbo.EmployeeInfoes");
            DropTable("dbo.DepartmentRoles");
            DropTable("dbo.DepartmentLocations");
            DropTable("dbo.Departments");
        }
    }
}
