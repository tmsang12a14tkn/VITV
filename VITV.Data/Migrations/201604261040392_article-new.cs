namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class articlenew : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ArticleCommentReplies");
            AddColumn("dbo.Articles", "ArticleDraft", c => c.String());
            AlterColumn("dbo.ArticleCommentReplies", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ArticleCommentReplies", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ArticleCommentReplies");
            AlterColumn("dbo.ArticleCommentReplies", "Id", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Articles", "ArticleDraft");
            AddPrimaryKey("dbo.ArticleCommentReplies", "Id");
        }
    }
}
