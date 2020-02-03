namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addrate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Currencies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrencyCode = c.String(),
                        CurrencyName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VNDExchangeRates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrencyId = c.Int(nullable: false),
                        Buy = c.Double(nullable: false),
                        Transfer = c.Double(nullable: false),
                        Sell = c.Double(nullable: false),
                        UpdatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Currencies", t => t.CurrencyId, cascadeDelete: true)
                .Index(t => t.CurrencyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VNDExchangeRates", "FK_VNDExchangeRates_Currencies_CurrencyId");
            DropIndex("dbo.VNDExchangeRates", new[] { "CurrencyId" });
            DropTable("dbo.VNDExchangeRates");
            DropTable("dbo.Currencies");
        }
    }
}
