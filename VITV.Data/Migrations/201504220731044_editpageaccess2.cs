namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editpageaccess2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PageAccesses", "DeviceType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PageAccesses", "DeviceType");
        }
    }
}
