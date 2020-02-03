namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatepageaccess : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pages", "VideoId", c => c.Int());
            AddColumn("dbo.Pages", "VideoCatId", c => c.Int());
            CreateIndex("dbo.Pages", "VideoId");
            CreateIndex("dbo.Pages", "VideoCatId");
            AddForeignKey("dbo.Pages", "VideoId", "dbo.Videos", "Id");
            AddForeignKey("dbo.Pages", "VideoCatId", "dbo.VideoCategories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pages", "VideoCatId", "dbo.VideoCategories");
            DropForeignKey("dbo.Pages", "VideoId", "dbo.Videos");
            DropIndex("Pages", new[] { "VideoCatId" });
            DropIndex("Pages", new[] { "VideoId" });
            DropColumn("dbo.Pages", "VideoCatId");
            DropColumn("dbo.Pages", "VideoId");
        }
    }
}
