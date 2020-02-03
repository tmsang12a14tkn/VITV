namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class positionreporter : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PositionLevels", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.PositionLevels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Reporters", "PositionId", c => c.Int());
            CreateIndex("dbo.Reporters", "PositionId");
            AddForeignKey("dbo.Reporters", "PositionId", "dbo.Positions", "Id");
            DropColumn("dbo.Reporters", "Position");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reporters", "Position", c => c.String(nullable: false));
            DropForeignKey("dbo.Reporters", "FK_Reporters_Positions_PositionId");
            DropForeignKey("dbo.Positions", "FK_Positions_PositionLevels_GroupId");
            DropIndex("dbo.Positions", new[] { "GroupId" });
            DropIndex("dbo.Reporters", new[] { "PositionId" });
            DropColumn("dbo.Reporters", "PositionId");
            DropTable("dbo.PositionLevels");
            DropTable("dbo.Positions");
        }
    }
}
