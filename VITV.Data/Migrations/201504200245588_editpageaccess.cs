namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editpageaccess : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PageAccesses", "DeviceType", c => c.Int(nullable: false));
            AddColumn("dbo.PageAccesses", "DeviceVendor", c => c.String());
            AddColumn("dbo.PageAccesses", "OSName", c => c.String());
            AddColumn("dbo.PageAccesses", "BrowserName", c => c.String());
            AddColumn("dbo.PageAccesses", "DeviceCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PageAccesses", "DeviceCount");
            DropColumn("dbo.PageAccesses", "BrowserName");
            DropColumn("dbo.PageAccesses", "OSName");
            DropColumn("dbo.PageAccesses", "DeviceVendor");
            DropColumn("dbo.PageAccesses", "DeviceType");
        }
    }
}
