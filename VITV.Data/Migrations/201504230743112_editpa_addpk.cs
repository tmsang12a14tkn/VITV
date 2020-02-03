namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editpa_addpk : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.PageAccesses");
            AlterColumn("dbo.PageAccesses", "OSName", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.PageAccesses", new[] { "PageUrl", "IPAdress", "Date", "DeviceType", "DeviceModel", "OSName" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.PageAccesses");
            AlterColumn("dbo.PageAccesses", "OSName", c => c.String());
            AddPrimaryKey("dbo.PageAccesses", new[] { "PageUrl", "IPAdress", "Date", "DeviceType", "DeviceModel" });
        }
    }
}
