namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editpageaccess1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PageAccesses", "DeviceModel", c => c.String());
            DropColumn("dbo.PageAccesses", "DeviceType");
            DropColumn("dbo.PageAccesses", "DeviceCount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PageAccesses", "DeviceCount", c => c.Int(nullable: false));
            AddColumn("dbo.PageAccesses", "DeviceType", c => c.Int(nullable: false));
            DropColumn("dbo.PageAccesses", "DeviceModel");
        }
    }
}
