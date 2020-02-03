namespace VITV_Web.MigrationsStoreInfo
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edittableGoldPriceVietNam1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GoldPriceVietNam_Day", "IsShow", c => c.Int(nullable: false));
            AddColumn("dbo.GoldPriceVietNams", "IsShow", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GoldPriceVietNams", "IsShow");
            DropColumn("dbo.GoldPriceVietNam_Day", "IsShow");
        }
    }
}
