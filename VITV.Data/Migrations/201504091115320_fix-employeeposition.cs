namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixemployeeposition : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Positions", "PositionLevel_Id", "dbo.PositionLevels");
            DropIndex("Positions", new[] { "PositionLevel_Id" });
            RenameColumn(table: "dbo.Positions", name: "PositionLevel_Id", newName: "PositionLevelId");
            AlterColumn("dbo.Positions", "PositionLevelId", c => c.Int(nullable: false));
            CreateIndex("dbo.Positions", "PositionLevelId");
            AddForeignKey("dbo.Positions", "PositionLevelId", "dbo.PositionLevels", "Id", cascadeDelete: true);
            DropColumn("dbo.Positions", "GroupId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Positions", "GroupId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Positions", "PositionLevelId", "dbo.PositionLevels");
            DropIndex("Positions", new[] { "PositionLevelId" });
            AlterColumn("dbo.Positions", "PositionLevelId", c => c.Int());
            RenameColumn(table: "dbo.Positions", name: "PositionLevelId", newName: "PositionLevel_Id");
            CreateIndex("dbo.Positions", "PositionLevel_Id");
            AddForeignKey("dbo.Positions", "PositionLevel_Id", "dbo.PositionLevels", "Id");
        }
    }
}
