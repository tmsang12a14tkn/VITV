namespace VITV_Web.MigrationsStoreInfo
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditStockWorld : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.StockWorld_Day");
            DropPrimaryKey("dbo.StockWorlds");
            AddColumn("dbo.StockWorld_Day", "LastUpdate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.StockWorld_Day", "Name", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.StockWorlds", "Name", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.StockWorld_Day", new[] { "Name", "VCreateDate" });
            AddPrimaryKey("dbo.StockWorlds", "Name");
            DropColumn("dbo.StockWorld_Day", "Id");
            DropColumn("dbo.StockWorlds", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StockWorlds", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.StockWorld_Day", "Id", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.StockWorlds");
            DropPrimaryKey("dbo.StockWorld_Day");
            AlterColumn("dbo.StockWorlds", "Name", c => c.String());
            AlterColumn("dbo.StockWorld_Day", "Name", c => c.String());
            DropColumn("dbo.StockWorld_Day", "LastUpdate");
            AddPrimaryKey("dbo.StockWorlds", "Id");
            AddPrimaryKey("dbo.StockWorld_Day", "Id");
        }
    }
}
