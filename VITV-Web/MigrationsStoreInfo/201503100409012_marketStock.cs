namespace VITV_Web.MigrationsStoreInfo
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class marketStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockMarkets", "Ticker", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StockMarkets", "Ticker");
        }
    }
}
