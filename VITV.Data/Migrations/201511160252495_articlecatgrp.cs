namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class articlecatgrp : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.Id)
                .Index(t => t.UniqueTitle, unique: true, name: "TitleUniqueIndex");
            
            AddColumn("dbo.ArticleCategories", "GroupId", c => c.Int());
            CreateIndex("dbo.ArticleCategories", "GroupId");
            AddForeignKey("dbo.ArticleCategories", "GroupId", "dbo.ArticleCatGroups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArticleCategories", "GroupId", "dbo.ArticleCatGroups");
            DropIndex("ArticleCatGroups", "TitleUniqueIndex");
            DropIndex("ArticleCategories", new[] { "GroupId" });
            DropColumn("dbo.ArticleCategories", "GroupId");
            DropTable("dbo.ArticleCatGroups");
        }
    }
}
