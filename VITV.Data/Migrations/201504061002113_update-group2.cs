namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updategroup2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Positions", "GroupId", "dbo.PositionGroups");
            DropForeignKey("dbo.EmployeeWorkInfoes", "PositionGroupId", "dbo.PositionGroups");
            DropIndex("Positions", new[] { "GroupId" });
            DropIndex("EmployeeWorkInfoes", new[] { "PositionGroupId" });
            CreateTable(
                "dbo.PositionLevels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PositionLevelGroups",
                c => new
                    {
                        PositionLevel_Id = c.Int(nullable: false),
                        Group_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PositionLevel_Id, t.Group_Id })
                .ForeignKey("dbo.PositionLevels", t => t.PositionLevel_Id, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .Index(t => t.PositionLevel_Id)
                .Index(t => t.Group_Id);
            
            AddColumn("dbo.Employees", "PositionLevel_Id", c => c.Int());
            AddColumn("dbo.Positions", "PositionLevel_Id", c => c.Int());
            AddColumn("dbo.EmployeeWorkInfoes", "PositionLevelId", c => c.Int(nullable: false));
            CreateIndex("dbo.Employees", "PositionLevel_Id");
            CreateIndex("dbo.Positions", "PositionLevel_Id");
            CreateIndex("dbo.EmployeeWorkInfoes", "PositionLevelId");
            AddForeignKey("dbo.Employees", "PositionLevel_Id", "dbo.PositionLevels", "Id");
            AddForeignKey("dbo.Positions", "PositionLevel_Id", "dbo.PositionLevels", "Id");
            AddForeignKey("dbo.EmployeeWorkInfoes", "PositionLevelId", "dbo.PositionLevels", "Id", cascadeDelete: true);
            DropColumn("dbo.EmployeeWorkInfoes", "PositionGroupId");
            DropTable("dbo.PositionGroups");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PositionGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.EmployeeWorkInfoes", "PositionGroupId", c => c.Int(nullable: false));
            DropForeignKey("dbo.EmployeeWorkInfoes", "PositionLevelId", "dbo.PositionLevels");
            DropForeignKey("dbo.Positions", "PositionLevel_Id", "dbo.PositionLevels");
            DropForeignKey("dbo.PositionLevelGroups", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.PositionLevelGroups", "PositionLevel_Id", "dbo.PositionLevels");
            DropForeignKey("dbo.Employees", "PositionLevel_Id", "dbo.PositionLevels");
            DropIndex("PositionLevelGroups", new[] { "Group_Id" });
            DropIndex("PositionLevelGroups", new[] { "PositionLevel_Id" });
            DropIndex("EmployeeWorkInfoes", new[] { "PositionLevelId" });
            DropIndex("Positions", new[] { "PositionLevel_Id" });
            DropIndex("Employees", new[] { "PositionLevel_Id" });
            DropColumn("dbo.EmployeeWorkInfoes", "PositionLevelId");
            DropColumn("dbo.Positions", "PositionLevel_Id");
            DropColumn("dbo.Employees", "PositionLevel_Id");
            DropTable("dbo.PositionLevelGroups");
            DropTable("dbo.PositionLevels");
            CreateIndex("dbo.EmployeeWorkInfoes", "PositionGroupId");
            CreateIndex("dbo.Positions", "GroupId");
            AddForeignKey("dbo.EmployeeWorkInfoes", "PositionGroupId", "dbo.PositionGroups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Positions", "GroupId", "dbo.PositionGroups", "Id", cascadeDelete: true);
        }
    }
}
