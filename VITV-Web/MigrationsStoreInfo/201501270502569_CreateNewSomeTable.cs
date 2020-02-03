namespace VITV_Web.MigrationsStoreInfo
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateNewSomeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GoldPriceVietNam_Day",
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
            
            CreateTable(
                "dbo.GoldPriceVietNams",
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
            
            CreateTable(
                "dbo.GoldPriceWorld_Day",
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
            
            CreateTable(
                "dbo.GoldPriceWorlds",
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
            
            CreateTable(
                "dbo.MetalPrice_Day",
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
            
            CreateTable(
                "dbo.MetalPrices",
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
            
            CreateTable(
                "dbo.OilPrice_Day",
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
            
            CreateTable(
                "dbo.OilPrices",
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
            DropTable("dbo.OilPrices");
            DropTable("dbo.OilPrice_Day");
            DropTable("dbo.MetalPrices");
            DropTable("dbo.MetalPrice_Day");
            DropTable("dbo.GoldPriceWorlds");
            DropTable("dbo.GoldPriceWorld_Day");
            DropTable("dbo.GoldPriceVietNams");
            DropTable("dbo.GoldPriceVietNam_Day");
        }
    }
}
