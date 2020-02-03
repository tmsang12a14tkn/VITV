namespace VITV.Data.MigrationsStoreInfo
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stockmarket_realtime : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StockMarket_RealTime",
                c => new
                    {
                        Ticker = c.String(nullable: false, maxLength: 128),
                        Close = c.Single(nullable: false),
                        Volumn = c.Int(nullable: false),
                        PriceChange = c.Single(nullable: false),
                        PercentChange = c.Single(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        IsShown = c.Boolean(nullable: false),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Ticker);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StockMarket_RealTime");
        }
    }
}
