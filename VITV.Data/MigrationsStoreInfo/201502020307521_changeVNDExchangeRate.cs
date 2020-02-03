namespace VITV.Data.MigrationsStoreInfo
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeVNDExchangeRate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VNDExchangeRate_Day", "Buy", c => c.Double(nullable: false));
            AddColumn("dbo.VNDExchangeRate_Day", "Sell", c => c.Double(nullable: false));
            AddColumn("dbo.VNDExchangeRate_Day", "Transfer", c => c.Double(nullable: false));
            AddColumn("dbo.VNDExchangeRate_Day", "UpdatedTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.VNDExchangeRates", "Buy", c => c.Double(nullable: false));
            AddColumn("dbo.VNDExchangeRates", "Sell", c => c.Double(nullable: false));
            AddColumn("dbo.VNDExchangeRates", "Transfer", c => c.Double(nullable: false));
            AddColumn("dbo.VNDExchangeRates", "UpdatedTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.VNDExchangeRate_Day", "Value");
            DropColumn("dbo.VNDExchangeRate_Day", "Change");
            DropColumn("dbo.VNDExchangeRate_Day", "Percent");
            DropColumn("dbo.VNDExchangeRate_Day", "VCreateDate");
            DropColumn("dbo.VNDExchangeRates", "Value");
            DropColumn("dbo.VNDExchangeRates", "Change");
            DropColumn("dbo.VNDExchangeRates", "Percent");
            DropColumn("dbo.VNDExchangeRates", "VCreateDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VNDExchangeRates", "VCreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.VNDExchangeRates", "Percent", c => c.Double(nullable: false));
            AddColumn("dbo.VNDExchangeRates", "Change", c => c.Double(nullable: false));
            AddColumn("dbo.VNDExchangeRates", "Value", c => c.Double(nullable: false));
            AddColumn("dbo.VNDExchangeRate_Day", "VCreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.VNDExchangeRate_Day", "Percent", c => c.Double(nullable: false));
            AddColumn("dbo.VNDExchangeRate_Day", "Change", c => c.Double(nullable: false));
            AddColumn("dbo.VNDExchangeRate_Day", "Value", c => c.Double(nullable: false));
            DropColumn("dbo.VNDExchangeRates", "UpdatedTime");
            DropColumn("dbo.VNDExchangeRates", "Transfer");
            DropColumn("dbo.VNDExchangeRates", "Sell");
            DropColumn("dbo.VNDExchangeRates", "Buy");
            DropColumn("dbo.VNDExchangeRate_Day", "UpdatedTime");
            DropColumn("dbo.VNDExchangeRate_Day", "Transfer");
            DropColumn("dbo.VNDExchangeRate_Day", "Sell");
            DropColumn("dbo.VNDExchangeRate_Day", "Buy");
        }
    }
}
