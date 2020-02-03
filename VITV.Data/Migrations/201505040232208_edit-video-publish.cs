namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editvideopublish : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "PublishImmediately", c => c.Boolean(nullable: false));
            AddColumn("dbo.Videos", "IsPublished", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Videos", "IsPublished");
            DropColumn("dbo.Videos", "PublishImmediately");
        }
    }
}
