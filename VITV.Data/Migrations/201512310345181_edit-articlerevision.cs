namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editarticlerevision : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArticleRevisions", "Title", c => c.String());
            AddColumn("dbo.ArticleRevisions", "ShortBrief", c => c.String());
            DropColumn("dbo.ArticleRevisions", "Order");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ArticleRevisions", "Order", c => c.Int(nullable: false));
            DropColumn("dbo.ArticleRevisions", "ShortBrief");
            DropColumn("dbo.ArticleRevisions", "Title");
        }
    }
}
