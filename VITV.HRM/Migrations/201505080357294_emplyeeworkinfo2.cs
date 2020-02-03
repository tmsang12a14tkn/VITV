namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class emplyeeworkinfo2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employees", "PositionLevelId", "dbo.PositionLevels");
            DropForeignKey("dbo.Employees", "GroupId", "dbo.Groups");
            DropIndex("dbo.Employees", new[] { "PositionLevel_Id" });
            DropIndex("dbo.Employees", new[] { "Group_Id" });
            DropColumn("dbo.Employees", "PositionLevel_Id");
            DropColumn("dbo.Employees", "Group_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "Group_Id", c => c.Int());
            AddColumn("dbo.Employees", "PositionLevel_Id", c => c.Int());
            CreateIndex("dbo.Employees", "Group_Id");
            CreateIndex("dbo.Employees", "PositionLevel_Id");
            AddForeignKey("dbo.Employees", "Group_Id", "dbo.Groups", "Id");
            AddForeignKey("dbo.Employees", "PositionLevel_Id", "dbo.PositionLevels", "Id");
        }
    }
}
