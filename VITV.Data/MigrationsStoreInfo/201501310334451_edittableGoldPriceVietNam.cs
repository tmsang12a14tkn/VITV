namespace VITV.Data.MigrationsStoreInfo
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edittableGoldPriceVietNam : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GoldPriceVietNam_Day", "Buy", c => c.Double(nullable: false));
            AddColumn("dbo.GoldPriceVietNam_Day", "Sell", c => c.Double(nullable: false));
            AddColumn("dbo.GoldPriceVietNams", "Buy", c => c.Double(nullable: false));
            AddColumn("dbo.GoldPriceVietNams", "Sell", c => c.Double(nullable: false));
            DropColumn("dbo.GoldPriceVietNam_Day", "Value");
            DropColumn("dbo.GoldPriceVietNam_Day", "Change");
            DropColumn("dbo.GoldPriceVietNam_Day", "Percent");
            DropColumn("dbo.GoldPriceVietNams", "Value");
            DropColumn("dbo.GoldPriceVietNams", "Change");
            DropColumn("dbo.GoldPriceVietNams", "Percent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GoldPriceVietNams", "Percent", c => c.Double(nullable: false));
            AddColumn("dbo.GoldPriceVietNams", "Change", c => c.Double(nullable: false));
            AddColumn("dbo.GoldPriceVietNams", "Value", c => c.Double(nullable: false));
            AddColumn("dbo.GoldPriceVietNam_Day", "Percent", c => c.Double(nullable: false));
            AddColumn("dbo.GoldPriceVietNam_Day", "Change", c => c.Double(nullable: false));
            AddColumn("dbo.GoldPriceVietNam_Day", "Value", c => c.Double(nullable: false));
            DropColumn("dbo.GoldPriceVietNams", "Sell");
            DropColumn("dbo.GoldPriceVietNams", "Buy");
            DropColumn("dbo.GoldPriceVietNam_Day", "Sell");
            DropColumn("dbo.GoldPriceVietNam_Day", "Buy");
        }
    }
}
