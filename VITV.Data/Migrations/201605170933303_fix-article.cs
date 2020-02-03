namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixarticle : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Articles", "ArticleContent", c => c.String());
            AlterColumn("dbo.Articles", "PublishedTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Articles", "PublishedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Articles", "ArticleContent", c => c.String(nullable: false));
        }
    }
}
