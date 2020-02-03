namespace VITV_Web.MigrationsStoreInfo
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editstockworld2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StockWorld_Day", "Symbol", c => c.String());
            AddColumn("dbo.StockWorlds", "Symbol", c => c.String());
            AddColumn("dbo.StockWorlds", "Volumn", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StockWorlds", "Volumn");
            DropColumn("dbo.StockWorlds", "Symbol");
            DropColumn("dbo.StockWorld_Day", "Symbol");
        }
    }
}
