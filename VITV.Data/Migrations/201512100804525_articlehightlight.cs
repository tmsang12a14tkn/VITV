namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class articlehightlight : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArticleHighlightAlls",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        HighlightType = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.ArticleHighlightCats",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        HighlightType = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArticleHighlightCats", "Id", "dbo.Articles");
            DropForeignKey("dbo.ArticleHighlightAlls", "Id", "dbo.Articles");
            DropIndex("ArticleHighlightCats", new[] { "Id" });
            DropIndex("ArticleHighlightAlls", new[] { "Id" });
            DropTable("dbo.ArticleHighlightCats");
            DropTable("dbo.ArticleHighlightAlls");
        }
    }
}
