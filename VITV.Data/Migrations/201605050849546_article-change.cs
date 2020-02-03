namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class articlechange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "CreatedTime", c => c.DateTime());
            DropColumn("dbo.Articles", "PublishImmediately");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articles", "PublishImmediately", c => c.Boolean(nullable: false));
            DropColumn("dbo.Articles", "CreatedTime");
        }
    }
}
