namespace VITV.Data.MigrationsStoreInfo
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edittableOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GoldPriceVietNam_Day", "IsShow", c => c.Int(nullable: false));
            AddColumn("dbo.GoldPriceVietNam_Day", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.GoldPriceWorld_Day", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.GoldPriceWorlds", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.MetalPrice_Day", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.MetalPrices", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.OilPrice_Day", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.OilPrices", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.USDExchangeRate_Day", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.USDExchangeRates", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.VNDExchangeRate_Day", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.VNDExchangeRates", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VNDExchangeRates", "Order");
            DropColumn("dbo.VNDExchangeRate_Day", "Order");
            DropColumn("dbo.USDExchangeRates", "Order");
            DropColumn("dbo.USDExchangeRate_Day", "Order");
            DropColumn("dbo.OilPrices", "Order");
            DropColumn("dbo.OilPrice_Day", "Order");
            DropColumn("dbo.MetalPrices", "Order");
            DropColumn("dbo.MetalPrice_Day", "Order");
            DropColumn("dbo.GoldPriceWorlds", "Order");
            DropColumn("dbo.GoldPriceWorld_Day", "Order");
            DropColumn("dbo.GoldPriceVietNam_Day", "Order");
            DropColumn("dbo.GoldPriceVietNam_Day", "IsShow");
        }
    }
}
