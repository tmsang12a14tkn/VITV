namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTableSpecialEvent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SpecialEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Date = c.DateTime(nullable: false),
                        HomeBkgr = c.String(nullable: true),
                        VideoBkgr = c.String(nullable: true),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Videos", "SpecialEventId", c => c.Int());
            AddColumn("dbo.Videos", "IsSpecial", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Videos", "SpecialEventId");
            AddForeignKey("dbo.Videos", "SpecialEventId", "dbo.SpecialEvents", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Videos", "FK_Videos_SpecialEvents_SpecialEventId");
            DropIndex("dbo.Videos", new[] { "SpecialEventId" });
            DropColumn("dbo.Videos", "IsSpecial");
            DropColumn("dbo.Videos", "SpecialEventId");
            DropTable("dbo.SpecialEvents");
        }
    }
}
