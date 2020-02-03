namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class articleAccess : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArticleCategoryAccesses",
                c => new
                    {
                        CategoryId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        IPViewCount = c.Int(nullable: false),
                        PageViewCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CategoryId, t.Date })
                .ForeignKey("dbo.ArticleCategories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            AddColumn("dbo.Pages", "ArticleId", c => c.Int());
            AddColumn("dbo.Pages", "ArticleCatId", c => c.Int());
            CreateIndex("dbo.Pages", "ArticleId");
            CreateIndex("dbo.Pages", "ArticleCatId");
            AddForeignKey("dbo.Pages", "ArticleId", "dbo.Articles", "Id");
            AddForeignKey("dbo.Pages", "ArticleCatId", "dbo.ArticleCategories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pages", "ArticleCatId", "dbo.ArticleCategories");
            DropForeignKey("dbo.Pages", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.ArticleCategoryAccesses", "CategoryId", "dbo.ArticleCategories");
            DropIndex("Pages", new[] { "ArticleCatId" });
            DropIndex("Pages", new[] { "ArticleId" });
            DropIndex("ArticleCategoryAccesses", new[] { "CategoryId" });
            DropColumn("dbo.Pages", "ArticleCatId");
            DropColumn("dbo.Pages", "ArticleId");
            DropTable("dbo.ArticleCategoryAccesses");
        }
    }
}
