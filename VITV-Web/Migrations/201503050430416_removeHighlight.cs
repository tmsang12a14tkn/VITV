namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeHighlight : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VideoCategoryHighlights", "FK_VideoCategoryHighlights_VideoCategories_VideoCategoryId");
            DropIndex("dbo.VideoCategoryHighlights", new[] { "VideoCategoryId" });
            DropTable("dbo.VideoCategoryHighlights");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.VideoCategoryHighlights",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(nullable: false),
                        ContentBonus = c.String(),
                        Photo = c.String(nullable: false),
                        VideoCategoryId = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        IsHot = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.VideoCategoryHighlights", "VideoCategoryId");
            AddForeignKey("dbo.VideoCategoryHighlights", "VideoCategoryId", "dbo.VideoCategories", "Id", cascadeDelete: true);
        }
    }
}
