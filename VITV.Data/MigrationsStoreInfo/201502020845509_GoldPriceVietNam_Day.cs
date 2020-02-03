namespace VITV.Data.MigrationsStoreInfo
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GoldPriceVietNam_Day : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.GoldPriceVietNam_Day", "IsShow");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GoldPriceVietNam_Day", "IsShow", c => c.Int(nullable: false));
        }
    }
}
