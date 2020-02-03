namespace VITV_Web.MigrationsStoreInfo
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateVNDExchangeRate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VNDExchangeRate_Day", "Code", c => c.String());
            AddColumn("dbo.VNDExchangeRates", "Code", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VNDExchangeRates", "Code");
            DropColumn("dbo.VNDExchangeRate_Day", "Code");
        }
    }
}
