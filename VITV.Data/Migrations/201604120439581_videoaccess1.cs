namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class videoaccess1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ArticleArticleCategories", newName: "ArticleCategoryArticles");
            DropPrimaryKey("dbo.ArticleCategoryArticles");
            CreateTable(
                "dbo.ArticleAccesses",
                c => new
                    {
                        ArticleId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        IPViewCount = c.Int(nullable: false),
                        PageViewCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ArticleId, t.Date })
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .Index(t => t.ArticleId);
            
            CreateTable(
                "dbo.VideoAccesses",
                c => new
                    {
                        VideoId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        IPViewCount = c.Int(nullable: false),
                        PageViewCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VideoId, t.Date })
                .ForeignKey("dbo.Videos", t => t.VideoId, cascadeDelete: true)
                .Index(t => t.VideoId);
            
            AddPrimaryKey("dbo.ArticleCategoryArticles", new[] { "ArticleCategory_Id", "Article_Id" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VideoAccesses", "VideoId", "dbo.Videos");
            DropForeignKey("dbo.ArticleAccesses", "ArticleId", "dbo.Articles");
            DropIndex("VideoAccesses", new[] { "VideoId" });
            DropIndex("ArticleAccesses", new[] { "ArticleId" });
            DropPrimaryKey("dbo.ArticleCategoryArticles");
            DropTable("dbo.VideoAccesses");
            DropTable("dbo.ArticleAccesses");
            AddPrimaryKey("dbo.ArticleCategoryArticles", new[] { "Article_Id", "ArticleCategory_Id" });
            RenameTable(name: "dbo.ArticleCategoryArticles", newName: "ArticleArticleCategories");
        }
    }
}
