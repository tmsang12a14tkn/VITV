namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editpa_pk : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.PageAccesses");
            AlterColumn("dbo.PageAccesses", "DeviceType", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.PageAccesses", "DeviceModel", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.PageAccesses", new[] { "PageUrl", "IPAdress", "Date", "DeviceType", "DeviceModel" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.PageAccesses");
            AlterColumn("dbo.PageAccesses", "DeviceModel", c => c.String());
            AlterColumn("dbo.PageAccesses", "DeviceType", c => c.String());
            AddPrimaryKey("dbo.PageAccesses", new[] { "PageUrl", "IPAdress", "Date" });
        }
    }
}
