namespace VITV.Data.MigrationsStoreInfo
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeGoldPrice : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.GoldPriceVietNam_Day");
            DropPrimaryKey("dbo.GoldPriceVietNams");
            AddColumn("dbo.GoldPriceVietNams", "YesterdayBuy", c => c.Double(nullable: false));
            AddColumn("dbo.GoldPriceVietNams", "YesterdaySell", c => c.Double(nullable: false));
            AlterColumn("dbo.GoldPriceVietNam_Day", "Name", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.GoldPriceVietNams", "Name", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.GoldPriceVietNam_Day", new[] { "Name", "VCreateDate" });
            AddPrimaryKey("dbo.GoldPriceVietNams", "Name");
            DropColumn("dbo.GoldPriceVietNam_Day", "Id");
            DropColumn("dbo.GoldPriceVietNams", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GoldPriceVietNams", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.GoldPriceVietNam_Day", "Id", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.GoldPriceVietNams");
            DropPrimaryKey("dbo.GoldPriceVietNam_Day");
            AlterColumn("dbo.GoldPriceVietNams", "Name", c => c.String());
            AlterColumn("dbo.GoldPriceVietNam_Day", "Name", c => c.String());
            DropColumn("dbo.GoldPriceVietNams", "YesterdaySell");
            DropColumn("dbo.GoldPriceVietNams", "YesterdayBuy");
            AddPrimaryKey("dbo.GoldPriceVietNams", "Id");
            AddPrimaryKey("dbo.GoldPriceVietNam_Day", "Id");
        }
    }
}
