namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editHighlight : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VideoCategoryHighlights", "IsHot", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VideoCategoryHighlights", "IsHot");
        }
    }
}
