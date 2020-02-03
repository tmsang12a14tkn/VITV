namespace VITV_Web.MigrationsStoreInfo
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editGoldPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GoldPriceVietNams", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.GoldPriceVietNams", "Type", c => c.String());
            DropColumn("dbo.GoldPriceVietNam_Day", "IsShow");
            DropColumn("dbo.GoldPriceVietNam_Day", "Order");
            DropColumn("dbo.GoldPriceWorld_Day", "Order");
            DropColumn("dbo.MetalPrice_Day", "Order");
            DropColumn("dbo.OilPrice_Day", "Order");
            DropColumn("dbo.USDExchangeRate_Day", "Order");
            DropColumn("dbo.VNDExchangeRate_Day", "Order");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VNDExchangeRate_Day", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.USDExchangeRate_Day", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.OilPrice_Day", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.MetalPrice_Day", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.GoldPriceWorld_Day", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.GoldPriceVietNam_Day", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.GoldPriceVietNam_Day", "IsShow", c => c.Int(nullable: false));
            DropColumn("dbo.GoldPriceVietNams", "Type");
            DropColumn("dbo.GoldPriceVietNams", "Order");
        }
    }
}
