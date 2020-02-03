namespace VITV_Web.MigrationsStoreInfo
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VNDExchangeRates : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ExchangeRates", newName: "USDExchangeRates");
            CreateTable(
                "dbo.VNDExchangeRates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.Double(nullable: false),
                        Change = c.Double(nullable: false),
                        Percent = c.Double(nullable: false),
                        VCreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.VNDExchangeRates");
            RenameTable(name: "dbo.USDExchangeRates", newName: "ExchangeRates");
        }
    }
}
