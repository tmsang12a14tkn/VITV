namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixdeviceuser : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DeviceUsers", "ExpiredTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DeviceUsers", "ExpiredTime", c => c.String());
        }
    }
}
