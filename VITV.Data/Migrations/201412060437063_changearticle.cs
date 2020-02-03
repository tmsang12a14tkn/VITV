namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changearticle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "PublishImmediately", c => c.Boolean(nullable: false));
            AddColumn("dbo.Articles", "IsPublished", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "IsPublished");
            DropColumn("dbo.Articles", "PublishImmediately");
        }
    }
}
