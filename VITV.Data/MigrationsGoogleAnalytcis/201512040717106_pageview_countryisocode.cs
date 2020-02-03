namespace VITV.Data.MigrationsGoogleAnalytcis
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pageview_countryisocode : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CountryIsoCodes",
                c => new
                    {
                        CreateDate = c.DateTime(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128),
                        PageUrl = c.String(nullable: false, maxLength: 128),
                        VideoId = c.Int(),
                        VideoCatId = c.Int(),
                        SessionCount = c.Double(nullable: false),
                        NewSessionRate = c.Double(nullable: false),
                        NewUser = c.Double(nullable: false),
                        BounceRate = c.Double(nullable: false),
                        PageViewPerSession = c.Double(nullable: false),
                        AvgSessionDurration = c.Double(nullable: false),
                        PageView = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CreateDate, t.Name, t.PageUrl });
            
            AddColumn("dbo.AgeOverviews", "PageView", c => c.Int(nullable: false));
            AddColumn("dbo.Ages", "PageView", c => c.Int(nullable: false));
            AddColumn("dbo.Browsers", "PageView", c => c.Int(nullable: false));
            AddColumn("dbo.Cities", "PageView", c => c.Int(nullable: false));
            AddColumn("dbo.Continents", "PageView", c => c.Int(nullable: false));
            AddColumn("dbo.Countries", "PageView", c => c.Int(nullable: false));
            AddColumn("dbo.DeviceBrandings", "PageView", c => c.Int(nullable: false));
            AddColumn("dbo.DeviceCategories", "PageView", c => c.Int(nullable: false));
            AddColumn("dbo.DeviceInfoes", "PageView", c => c.Int(nullable: false));
            AddColumn("dbo.GenderOverviews", "PageView", c => c.Int(nullable: false));
            AddColumn("dbo.Genders", "PageView", c => c.Int(nullable: false));
            AddColumn("dbo.OS", "PageView", c => c.Int(nullable: false));
            AddColumn("dbo.Resolutions", "PageView", c => c.Int(nullable: false));
            AddColumn("dbo.SiteSearches", "PageView", c => c.Int(nullable: false));
            AddColumn("dbo.SubContinents", "PageView", c => c.Int(nullable: false));
            AddColumn("dbo.UserTypes", "PageView", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserTypes", "PageView");
            DropColumn("dbo.SubContinents", "PageView");
            DropColumn("dbo.SiteSearches", "PageView");
            DropColumn("dbo.Resolutions", "PageView");
            DropColumn("dbo.OS", "PageView");
            DropColumn("dbo.Genders", "PageView");
            DropColumn("dbo.GenderOverviews", "PageView");
            DropColumn("dbo.DeviceInfoes", "PageView");
            DropColumn("dbo.DeviceCategories", "PageView");
            DropColumn("dbo.DeviceBrandings", "PageView");
            DropColumn("dbo.Countries", "PageView");
            DropColumn("dbo.Continents", "PageView");
            DropColumn("dbo.Cities", "PageView");
            DropColumn("dbo.Browsers", "PageView");
            DropColumn("dbo.Ages", "PageView");
            DropColumn("dbo.AgeOverviews", "PageView");
            DropTable("dbo.CountryIsoCodes");
        }
    }
}
