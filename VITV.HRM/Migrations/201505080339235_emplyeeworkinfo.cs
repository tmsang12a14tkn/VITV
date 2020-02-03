namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class emplyeeworkinfo : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Employees", name: "GroupId", newName: "Group_Id");
            RenameColumn(table: "dbo.Employees", name: "PositionLevelId", newName: "PositionLevel_Id");
            RenameIndex(table: "dbo.Employees", name: "IX_PositionLevelId", newName: "IX_PositionLevel_Id");
            RenameIndex(table: "dbo.Employees", name: "IX_GroupId", newName: "IX_Group_Id");
            CreateTable(
                "dbo.EmployeeWorkInfoes",
                c => new
                    {
                        EmployeeId = c.String(nullable: false, maxLength: 128),
                        StartDate = c.DateTime(nullable: false),
                        GroupId = c.Int(),
                        PositionLevelId = c.Int(),
                    })
                .PrimaryKey(t => t.EmployeeId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .ForeignKey("dbo.Groups", t => t.GroupId)
                .ForeignKey("dbo.PositionLevels", t => t.PositionLevelId)
                .Index(t => t.EmployeeId)
                .Index(t => t.GroupId)
                .Index(t => t.PositionLevelId);
            
            CreateTable(
                "dbo.EmployeeSkills",
                c => new
                    {
                        EmployeeId = c.String(nullable: false, maxLength: 128),
                        SkillId = c.Int(nullable: false),
                        Rate = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EmployeeId, t.SkillId })
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.Skills", t => t.SkillId, cascadeDelete: true)
                .ForeignKey("dbo.EmployeeWorkInfoes", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId)
                .Index(t => t.SkillId);
            
            CreateTable(
                "dbo.Skills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Employees", "About", c => c.String());
            AddColumn("dbo.Employees", "BirthDay", c => c.DateTime(nullable: false));
            AddColumn("dbo.Employees", "Address", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmployeeSkills", "EmployeeId", "dbo.EmployeeWorkInfoes");
            DropForeignKey("dbo.EmployeeSkills", "SkillId", "dbo.Skills");
            DropForeignKey("dbo.EmployeeSkills", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.EmployeeWorkInfoes", "PositionLevelId", "dbo.PositionLevels");
            DropForeignKey("dbo.EmployeeWorkInfoes", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.EmployeeWorkInfoes", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.EmployeeSkills", new[] { "SkillId" });
            DropIndex("dbo.EmployeeSkills", new[] { "EmployeeId" });
            DropIndex("dbo.EmployeeWorkInfoes", new[] { "PositionLevelId" });
            DropIndex("dbo.EmployeeWorkInfoes", new[] { "GroupId" });
            DropIndex("dbo.EmployeeWorkInfoes", new[] { "EmployeeId" });
            DropColumn("dbo.Employees", "Address");
            DropColumn("dbo.Employees", "BirthDay");
            DropColumn("dbo.Employees", "About");
            DropTable("dbo.Skills");
            DropTable("dbo.EmployeeSkills");
            DropTable("dbo.EmployeeWorkInfoes");
            RenameIndex(table: "dbo.Employees", name: "IX_Group_Id", newName: "IX_GroupId");
            RenameIndex(table: "dbo.Employees", name: "IX_PositionLevel_Id", newName: "IX_PositionLevelId");
            RenameColumn(table: "dbo.Employees", name: "PositionLevel_Id", newName: "PositionLevelId");
            RenameColumn(table: "dbo.Employees", name: "Group_Id", newName: "GroupId");
        }
    }
}
