namespace VITV.Data.MigrationsStoreInfo
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditTableStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockWorld_Day", "VCreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.StockWorlds", "VCreateDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.StockWorld_Day", "UtcTime");
            DropColumn("dbo.StockWorld_Day", "IsShow");
            DropColumn("dbo.StockWorld_Day", "Order");
            DropColumn("dbo.StockWorlds", "UtcTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StockWorlds", "UtcTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.StockWorld_Day", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.StockWorld_Day", "IsShow", c => c.Int(nullable: false));
            AddColumn("dbo.StockWorld_Day", "UtcTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.StockWorlds", "VCreateDate");
            DropColumn("dbo.StockWorld_Day", "VCreateDate");
        }
    }
}
