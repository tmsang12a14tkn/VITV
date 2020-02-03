namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uniquetitlecat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArticleCategories", "UniqueTitle", c => c.String());
            AddColumn("dbo.VideoCategories", "UniqueTitle", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VideoCategories", "UniqueTitle");
            DropColumn("dbo.ArticleCategories", "UniqueTitle");
        }
    }
}
