namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hasads : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VideoCategories", "HasAds", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VideoCategories", "HasAds");
        }
    }
}
