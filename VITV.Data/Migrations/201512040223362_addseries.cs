namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addseries : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArticleSeries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UniqueTitle = c.String(maxLength: 450),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        ProfilePhoto = c.String(),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UniqueTitle, unique: true, name: "TitleUniqueIndex");
            
            AddColumn("dbo.Articles", "ArticleSeriesId", c => c.Int());
            CreateIndex("dbo.Articles", "ArticleSeriesId");
            AddForeignKey("dbo.Articles", "ArticleSeriesId", "dbo.ArticleSeries", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Articles", "ArticleSeriesId", "dbo.ArticleSeries");
            DropIndex("ArticleSeries", "TitleUniqueIndex");
            DropIndex("Articles", new[] { "ArticleSeriesId" });
            DropColumn("dbo.Articles", "ArticleSeriesId");
            DropTable("dbo.ArticleSeries");
        }
    }
}
