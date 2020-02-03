namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class holiday : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SpecialEvents", "BarBkgr", c => c.String());
            AddColumn("dbo.Holidays", "LeftFixedBkgr", c => c.String());
            AddColumn("dbo.Holidays", "RightFixedBkgr", c => c.String());
            DropColumn("dbo.SpecialEvents", "HomeBkgr");
            DropColumn("dbo.SpecialEvents", "VideoBkgr");
            DropColumn("dbo.Holidays", "HomeBkgr");
            DropColumn("dbo.Holidays", "VideoBkgr");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Holidays", "VideoBkgr", c => c.String());
            AddColumn("dbo.Holidays", "HomeBkgr", c => c.String());
            AddColumn("dbo.SpecialEvents", "VideoBkgr", c => c.String());
            AddColumn("dbo.SpecialEvents", "HomeBkgr", c => c.String());
            DropColumn("dbo.Holidays", "RightFixedBkgr");
            DropColumn("dbo.Holidays", "LeftFixedBkgr");
            DropColumn("dbo.SpecialEvents", "BarBkgr");
        }
    }
}
