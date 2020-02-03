namespace VITV.Data.MigrationsStoreInfo
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class USDExchangeRates_Day : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.USDExchangeRate_Day",
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
            DropTable("dbo.USDExchangeRate_Day");
        }
    }
}
