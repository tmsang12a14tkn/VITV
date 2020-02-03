namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renamenotification : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Notifications", newName: "GroupNotifications");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.GroupNotifications", newName: "Notifications");
        }
    }
}
