namespace VITV.Data.MigrationsStoreInfo
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditUSDExchangeRate : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.USDExchangeRate_Day");
            DropPrimaryKey("dbo.USDExchangeRates");
            AddColumn("dbo.USDExchangeRate_Day", "LastUpdate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.USDExchangeRate_Day", "Name", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.USDExchangeRates", "Name", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.USDExchangeRate_Day", new[] { "Name", "VCreateDate" });
            AddPrimaryKey("dbo.USDExchangeRates", "Name");
            DropColumn("dbo.USDExchangeRate_Day", "Id");
            DropColumn("dbo.USDExchangeRates", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.USDExchangeRates", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.USDExchangeRate_Day", "Id", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.USDExchangeRates");
            DropPrimaryKey("dbo.USDExchangeRate_Day");
            AlterColumn("dbo.USDExchangeRates", "Name", c => c.String());
            AlterColumn("dbo.USDExchangeRate_Day", "Name", c => c.String());
            DropColumn("dbo.USDExchangeRate_Day", "LastUpdate");
            AddPrimaryKey("dbo.USDExchangeRates", "Id");
            AddPrimaryKey("dbo.USDExchangeRate_Day", "Id");
        }
    }
}
