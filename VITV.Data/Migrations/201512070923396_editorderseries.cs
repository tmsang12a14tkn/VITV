namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editorderseries : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "SeriesOrder", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "SeriesOrder");
        }
    }
}
