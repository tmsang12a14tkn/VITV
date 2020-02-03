namespace VITV.Data.MigrationsGoogleAnalytcis
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ages",
                c => new
                    {
                        CreateDate = c.DateTime(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128),
                        PageUrl = c.String(nullable: false, maxLength: 128),
                        SessionCount = c.Double(nullable: false),
                        NewSessionRate = c.Double(nullable: false),
                        NewUser = c.Double(nullable: false),
                        BounceRate = c.Double(nullable: false),
                        PageViewPerSession = c.Double(nullable: false),
                        AvgSessionDurration = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.CreateDate, t.Name, t.PageUrl });
            
            CreateTable(
                "dbo.Browsers",
                c => new
                    {
                        CreateDate = c.DateTime(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128),
                        PageUrl = c.String(nullable: false, maxLength: 128),
                        SessionCount = c.Double(nullable: false),
                        NewSessionRate = c.Double(nullable: false),
                        NewUser = c.Double(nullable: false),
                        BounceRate = c.Double(nullable: false),
                        PageViewPerSession = c.Double(nullable: false),
                        AvgSessionDurration = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.CreateDate, t.Name, t.PageUrl });
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        CreateDate = c.DateTime(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128),
                        PageUrl = c.String(nullable: false, maxLength: 128),
                        SessionCount = c.Double(nullable: false),
                        NewSessionRate = c.Double(nullable: false),
                        NewUser = c.Double(nullable: false),
                        BounceRate = c.Double(nullable: false),
                        PageViewPerSession = c.Double(nullable: false),
                        AvgSessionDurration = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.CreateDate, t.Name, t.PageUrl });
            
            CreateTable(
                "dbo.Continents",
                c => new
                    {
                        CreateDate = c.DateTime(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128),
                        PageUrl = c.String(nullable: false, maxLength: 128),
                        SessionCount = c.Double(nullable: false),
                        NewSessionRate = c.Double(nullable: false),
                        NewUser = c.Double(nullable: false),
                        BounceRate = c.Double(nullable: false),
                        PageViewPerSession = c.Double(nullable: false),
                        AvgSessionDurration = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.CreateDate, t.Name, t.PageUrl });
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        CreateDate = c.DateTime(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128),
                        PageUrl = c.String(nullable: false, maxLength: 128),
                        SessionCount = c.Double(nullable: false),
                        NewSessionRate = c.Double(nullable: false),
                        NewUser = c.Double(nullable: false),
                        BounceRate = c.Double(nullable: false),
                        PageViewPerSession = c.Double(nullable: false),
                        AvgSessionDurration = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.CreateDate, t.Name, t.PageUrl });
            
            CreateTable(
                "dbo.DeviceBrandings",
                c => new
                    {
                        CreateDate = c.DateTime(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128),
                        PageUrl = c.String(nullable: false, maxLength: 128),
                        SessionCount = c.Double(nullable: false),
                        NewSessionRate = c.Double(nullable: false),
                        NewUser = c.Double(nullable: false),
                        BounceRate = c.Double(nullable: false),
                        PageViewPerSession = c.Double(nullable: false),
                        AvgSessionDurration = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.CreateDate, t.Name, t.PageUrl });
            
            CreateTable(
                "dbo.DeviceCategories",
                c => new
                    {
                        CreateDate = c.DateTime(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128),
                        PageUrl = c.String(nullable: false, maxLength: 128),
                        SessionCount = c.Double(nullable: false),
                        NewSessionRate = c.Double(nullable: false),
                        NewUser = c.Double(nullable: false),
                        BounceRate = c.Double(nullable: false),
                        PageViewPerSession = c.Double(nullable: false),
                        AvgSessionDurration = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.CreateDate, t.Name, t.PageUrl });
            
            CreateTable(
                "dbo.DeviceInfoes",
                c => new
                    {
                        CreateDate = c.DateTime(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128),
                        PageUrl = c.String(nullable: false, maxLength: 128),
                        SessionCount = c.Double(nullable: false),
                        NewSessionRate = c.Double(nullable: false),
                        NewUser = c.Double(nullable: false),
                        BounceRate = c.Double(nullable: false),
                        PageViewPerSession = c.Double(nullable: false),
                        AvgSessionDurration = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.CreateDate, t.Name, t.PageUrl });
            
            CreateTable(
                "dbo.Genders",
                c => new
                    {
                        CreateDate = c.DateTime(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128),
                        PageUrl = c.String(nullable: false, maxLength: 128),
                        SessionCount = c.Double(nullable: false),
                        NewSessionRate = c.Double(nullable: false),
                        NewUser = c.Double(nullable: false),
                        BounceRate = c.Double(nullable: false),
                        PageViewPerSession = c.Double(nullable: false),
                        AvgSessionDurration = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.CreateDate, t.Name, t.PageUrl });
            
            CreateTable(
                "dbo.OS",
                c => new
                    {
                        CreateDate = c.DateTime(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128),
                        PageUrl = c.String(nullable: false, maxLength: 128),
                        SessionCount = c.Double(nullable: false),
                        NewSessionRate = c.Double(nullable: false),
                        NewUser = c.Double(nullable: false),
                        BounceRate = c.Double(nullable: false),
                        PageViewPerSession = c.Double(nullable: false),
                        AvgSessionDurration = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.CreateDate, t.Name, t.PageUrl });
            
            CreateTable(
                "dbo.Resolutions",
                c => new
                    {
                        CreateDate = c.DateTime(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128),
                        PageUrl = c.String(nullable: false, maxLength: 128),
                        SessionCount = c.Double(nullable: false),
                        NewSessionRate = c.Double(nullable: false),
                        NewUser = c.Double(nullable: false),
                        BounceRate = c.Double(nullable: false),
                        PageViewPerSession = c.Double(nullable: false),
                        AvgSessionDurration = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.CreateDate, t.Name, t.PageUrl });
            
            CreateTable(
                "dbo.SiteContents",
                c => new
                    {
                        CreateDate = c.DateTime(nullable: false),
                        PageUrl = c.String(nullable: false, maxLength: 128),
                        PageViews = c.Double(nullable: false),
                        UniquePageViews = c.Double(nullable: false),
                        AvgTimeOnPage = c.Double(nullable: false),
                        Entrances = c.Double(nullable: false),
                        BounceRate = c.Double(nullable: false),
                        ExitRate = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.CreateDate, t.PageUrl });
            
            CreateTable(
                "dbo.SiteSearches",
                c => new
                    {
                        CreateDate = c.DateTime(nullable: false),
                        PageUrl = c.String(nullable: false, maxLength: 128),
                        TotalUniqueSearch = c.Double(nullable: false),
                        AvgSearchResultViews = c.Double(nullable: false),
                        SearchExitRate = c.Double(nullable: false),
                        PercentSearchRefinements = c.Double(nullable: false),
                        SearchDuration = c.Double(nullable: false),
                        AvgSearchDepth = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.CreateDate, t.PageUrl });
            
            CreateTable(
                "dbo.SocialNetworkDetails",
                c => new
                    {
                        CreateDate = c.DateTime(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128),
                        PageUrl = c.String(nullable: false, maxLength: 128),
                        SessionCount = c.Double(nullable: false),
                        PageView = c.Double(nullable: false),
                        PageViewPerSession = c.Double(nullable: false),
                        AvgSessionDurration = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.CreateDate, t.Name, t.PageUrl });
            
            CreateTable(
                "dbo.SocialNetworkGrps",
                c => new
                    {
                        CreateDate = c.DateTime(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128),
                        SessionCount = c.Double(nullable: false),
                        PageView = c.Double(nullable: false),
                        PageViewPerSession = c.Double(nullable: false),
                        AvgSessionDurration = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.CreateDate, t.Name });
            
            CreateTable(
                "dbo.SubContinents",
                c => new
                    {
                        CreateDate = c.DateTime(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128),
                        PageUrl = c.String(nullable: false, maxLength: 128),
                        SessionCount = c.Double(nullable: false),
                        NewSessionRate = c.Double(nullable: false),
                        NewUser = c.Double(nullable: false),
                        BounceRate = c.Double(nullable: false),
                        PageViewPerSession = c.Double(nullable: false),
                        AvgSessionDurration = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.CreateDate, t.Name, t.PageUrl });
            
            CreateTable(
                "dbo.UserTypes",
                c => new
                    {
                        CreateDate = c.DateTime(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128),
                        PageUrl = c.String(nullable: false, maxLength: 128),
                        SessionCount = c.Double(nullable: false),
                        NewSessionRate = c.Double(nullable: false),
                        NewUser = c.Double(nullable: false),
                        BounceRate = c.Double(nullable: false),
                        PageViewPerSession = c.Double(nullable: false),
                        AvgSessionDurration = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.CreateDate, t.Name, t.PageUrl });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserTypes");
            DropTable("dbo.SubContinents");
            DropTable("dbo.SocialNetworkGrps");
            DropTable("dbo.SocialNetworkDetails");
            DropTable("dbo.SiteSearches");
            DropTable("dbo.SiteContents");
            DropTable("dbo.Resolutions");
            DropTable("dbo.OS");
            DropTable("dbo.Genders");
            DropTable("dbo.DeviceInfoes");
            DropTable("dbo.DeviceCategories");
            DropTable("dbo.DeviceBrandings");
            DropTable("dbo.Countries");
            DropTable("dbo.Continents");
            DropTable("dbo.Cities");
            DropTable("dbo.Browsers");
            DropTable("dbo.Ages");
        }
    }
}
