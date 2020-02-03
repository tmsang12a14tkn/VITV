namespace VITV.Data.MigrationsStoreInfo
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeGoldPrice2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GoldPriceVietNam_Day", "LastUpdated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GoldPriceVietNam_Day", "LastUpdated");
        }
    }
}
