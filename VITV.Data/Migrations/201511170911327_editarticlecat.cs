namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editarticlecat : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Articles", "CategoryId", "dbo.ArticleCategories");
            DropForeignKey("dbo.ArticleCategories", "GroupId", "dbo.ArticleCatGroups");
            DropIndex("ArticleCategories", new[] { "GroupId" });
            DropIndex("Articles", new[] { "CategoryId" });
            DropIndex("ArticleCatGroups", "TitleUniqueIndex");
            CreateTable(
                "dbo.ArticleArticleCategories",
                c => new
                    {
                        Article_Id = c.Int(nullable: false),
                        ArticleCategory_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Article_Id, t.ArticleCategory_Id })
                .ForeignKey("dbo.Articles", t => t.Article_Id, cascadeDelete: true)
                .ForeignKey("dbo.ArticleCategories", t => t.ArticleCategory_Id, cascadeDelete: true)
                .Index(t => t.Article_Id)
                .Index(t => t.ArticleCategory_Id);
            
            DropColumn("dbo.ArticleCategories", "GroupId");
            DropColumn("dbo.Articles", "CategoryId");
            DropTable("dbo.ArticleCatGroups");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ArticleCatGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Order = c.Int(nullable: false),
                        UniqueTitle = c.String(maxLength: 450),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Articles", "CategoryId", c => c.Int(nullable: false));
            AddColumn("dbo.ArticleCategories", "GroupId", c => c.Int());
            DropForeignKey("dbo.ArticleArticleCategories", "ArticleCategory_Id", "dbo.ArticleCategories");
            DropForeignKey("dbo.ArticleArticleCategories", "Article_Id", "dbo.Articles");
            DropIndex("ArticleArticleCategories", new[] { "ArticleCategory_Id" });
            DropIndex("ArticleArticleCategories", new[] { "Article_Id" });
            DropTable("dbo.ArticleArticleCategories");
            CreateIndex("dbo.ArticleCatGroups", "UniqueTitle", unique: true, name: "TitleUniqueIndex");
            CreateIndex("dbo.Articles", "CategoryId");
            CreateIndex("dbo.ArticleCategories", "GroupId");
            AddForeignKey("dbo.ArticleCategories", "GroupId", "dbo.ArticleCatGroups", "Id");
            AddForeignKey("dbo.Articles", "CategoryId", "dbo.ArticleCategories", "Id", cascadeDelete: true);
        }
    }
}
