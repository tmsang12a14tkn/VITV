namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class articlecomment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArticleCommentReplies",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Content = c.String(),
                        CreatedTime = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                        ArticleCommentId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ArticleComments", t => t.ArticleCommentId)
                .ForeignKey("dbo.ApplicationUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ArticleCommentId);
            
            CreateTable(
                "dbo.ArticleComments",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Content = c.String(),
                        CreatedTime = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                        ArticleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.ApplicationUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ArticleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArticleCommentReplies", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ArticleComments", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ArticleCommentReplies", "ArticleCommentId", "dbo.ArticleComments");
            DropForeignKey("dbo.ArticleComments", "ArticleId", "dbo.Articles");
            DropIndex("ArticleComments", new[] { "ArticleId" });
            DropIndex("ArticleComments", new[] { "UserId" });
            DropIndex("ArticleCommentReplies", new[] { "ArticleCommentId" });
            DropIndex("ArticleCommentReplies", new[] { "UserId" });
            DropTable("dbo.ArticleComments");
            DropTable("dbo.ArticleCommentReplies");
        }
    }
}
