namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editArticle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Thumbnail", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "Thumbnail");
        }
    }
}
