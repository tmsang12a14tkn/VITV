namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editHighlight1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VideoCategoryHighlights", "ContentBonus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VideoCategoryHighlights", "ContentBonus");
        }
    }
}
