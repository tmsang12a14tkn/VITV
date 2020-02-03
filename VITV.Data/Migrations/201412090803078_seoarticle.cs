namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seoarticle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "SEO_FocusKeywords", c => c.String());
            AddColumn("dbo.Articles", "SEO_Title", c => c.String());
            AddColumn("dbo.Articles", "SEO_MetaDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "SEO_MetaDescription");
            DropColumn("dbo.Articles", "SEO_Title");
            DropColumn("dbo.Articles", "SEO_FocusKeywords");
        }
    }
}
