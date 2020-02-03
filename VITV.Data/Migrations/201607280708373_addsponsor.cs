namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsponsor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VideoCategorySponsors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Thumbnail = c.String(nullable: false),
                        Url = c.String(nullable: false),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VideoCategorySponsorVideoCategories",
                c => new
                    {
                        VideoCategorySponsor_Id = c.Int(nullable: false),
                        VideoCategory_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VideoCategorySponsor_Id, t.VideoCategory_Id })
                .ForeignKey("dbo.VideoCategorySponsors", t => t.VideoCategorySponsor_Id, cascadeDelete: true)
                .ForeignKey("dbo.VideoCategories", t => t.VideoCategory_Id, cascadeDelete: true)
                .Index(t => t.VideoCategorySponsor_Id)
                .Index(t => t.VideoCategory_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VideoCategorySponsorVideoCategories", "VideoCategory_Id", "dbo.VideoCategories");
            DropForeignKey("dbo.VideoCategorySponsorVideoCategories", "VideoCategorySponsor_Id", "dbo.VideoCategorySponsors");
            DropIndex("VideoCategorySponsorVideoCategories", new[] { "VideoCategory_Id" });
            DropIndex("VideoCategorySponsorVideoCategories", new[] { "VideoCategorySponsor_Id" });
            DropTable("dbo.VideoCategorySponsorVideoCategories");
            DropTable("dbo.VideoCategorySponsors");
        }
    }
}
