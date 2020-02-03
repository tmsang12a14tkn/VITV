namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editVideoHighlights : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VideoCategoryHighlights", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VideoCategoryHighlights", "Order");
        }
    }
}
