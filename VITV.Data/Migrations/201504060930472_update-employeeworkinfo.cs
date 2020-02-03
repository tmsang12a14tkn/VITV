namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateemployeeworkinfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmployeeWorkInfoes", "PositionLevelId", c => c.Int(nullable: false));
            AddColumn("dbo.EmployeeWorkInfoes", "PositionTitle", c => c.String());
            CreateIndex("dbo.EmployeeWorkInfoes", "PositionLevelId");
            AddForeignKey("dbo.EmployeeWorkInfoes", "PositionLevelId", "dbo.PositionLevels", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmployeeWorkInfoes", "PositionLevelId", "dbo.PositionLevels");
            DropIndex("EmployeeWorkInfoes", new[] { "PositionLevelId" });
            DropColumn("dbo.EmployeeWorkInfoes", "PositionTitle");
            DropColumn("dbo.EmployeeWorkInfoes", "PositionLevelId");
        }
    }
}
