namespace VITV.Data.MigrationsStoreInfo
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStockVN : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Stock_Day",
                c => new
                    {
                        Ticker = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        Open = c.Single(nullable: false),
                        Close = c.Single(nullable: false),
                        High = c.Single(nullable: false),
                        Low = c.Single(nullable: false),
                        Volumn = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ticker, t.Date })
                .ForeignKey("dbo.Stocks", t => t.Ticker, cascadeDelete: true)
                .Index(t => t.Ticker);
            
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        Ticker = c.String(nullable: false, maxLength: 128),
                        CompanyName = c.String(),
                        MarketName = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Ticker)
                .ForeignKey("dbo.StockMarkets", t => t.MarketName)
                .Index(t => t.MarketName);
            
            CreateTable(
                "dbo.StockMarkets",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.StockMarket_Day",
                c => new
                    {
                        MarketName = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        Open = c.Single(nullable: false),
                        Close = c.Single(nullable: false),
                        High = c.Single(nullable: false),
                        Low = c.Single(nullable: false),
                        Volumn = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MarketName, t.Date })
                .ForeignKey("dbo.StockMarkets", t => t.MarketName, cascadeDelete: true)
                .Index(t => t.MarketName);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StockMarket_Day", "MarketName", "dbo.StockMarkets");
            DropForeignKey("dbo.Stock_Day", "Ticker", "dbo.Stocks");
            DropForeignKey("dbo.Stocks", "MarketName", "dbo.StockMarkets");
            DropIndex("dbo.StockMarket_Day", new[] { "MarketName" });
            DropIndex("dbo.Stocks", new[] { "MarketName" });
            DropIndex("dbo.Stock_Day", new[] { "Ticker" });
            DropTable("dbo.StockMarket_Day");
            DropTable("dbo.StockMarkets");
            DropTable("dbo.Stocks");
            DropTable("dbo.Stock_Day");
        }
    }
}
