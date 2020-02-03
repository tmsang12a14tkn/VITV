namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class timeusernotifi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserNotifications", "CreatedTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserNotifications", "CreatedTime");
        }
    }
}
