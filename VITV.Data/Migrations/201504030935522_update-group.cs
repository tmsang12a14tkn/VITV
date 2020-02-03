namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updategroup : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.EmployeeInfoes", newName: "EmployeePersonalInfoes");
            DropForeignKey("dbo.Departments", "DepartmentLocationId", "dbo.DepartmentLocations");
            DropForeignKey("dbo.Departments", "DepartmentRoleId", "dbo.DepartmentRoles");
            DropForeignKey("dbo.Employees", "DepartmentId", "dbo.Departments");
            DropIndex("Employees", new[] { "DepartmentId" });
            DropIndex("Departments", new[] { "DepartmentRoleId" });
            DropIndex("Departments", new[] { "DepartmentLocationId" });
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .Index(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Agencies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmployeeWorkInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        AgencyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Agencies", t => t.AgencyId, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.AgencyId);
            
            AddColumn("dbo.Employees", "GroupId", c => c.Int());
            CreateIndex("dbo.Employees", "GroupId");
            AddForeignKey("dbo.Employees", "GroupId", "dbo.Groups", "Id");
            DropColumn("dbo.Employees", "DepartmentId");
            DropColumn("dbo.Departments", "DepartmentRoleId");
            DropColumn("dbo.Departments", "DepartmentLocationId");
            DropTable("dbo.DepartmentLocations");
            DropTable("dbo.DepartmentRoles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DepartmentRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DepartmentLocations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Departments", "DepartmentLocationId", c => c.Int(nullable: false));
            AddColumn("dbo.Departments", "DepartmentRoleId", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "DepartmentId", c => c.Int());
            DropForeignKey("dbo.EmployeeWorkInfoes", "Id", "dbo.Employees");
            DropForeignKey("dbo.EmployeeWorkInfoes", "AgencyId", "dbo.Agencies");
            DropForeignKey("dbo.Employees", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Groups", "DepartmentId", "dbo.Departments");
            DropIndex("EmployeeWorkInfoes", new[] { "AgencyId" });
            DropIndex("EmployeeWorkInfoes", new[] { "Id" });
            DropIndex("Groups", new[] { "DepartmentId" });
            DropIndex("Employees", new[] { "GroupId" });
            DropColumn("dbo.Employees", "GroupId");
            DropTable("dbo.EmployeeWorkInfoes");
            DropTable("dbo.Agencies");
            DropTable("dbo.Groups");
            CreateIndex("dbo.Departments", "DepartmentLocationId");
            CreateIndex("dbo.Departments", "DepartmentRoleId");
            CreateIndex("dbo.Employees", "DepartmentId");
            AddForeignKey("dbo.Employees", "DepartmentId", "dbo.Departments", "Id");
            AddForeignKey("dbo.Departments", "DepartmentRoleId", "dbo.DepartmentRoles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Departments", "DepartmentLocationId", "dbo.DepartmentLocations", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.EmployeePersonalInfoes", newName: "EmployeeInfoes");
        }
    }
}
