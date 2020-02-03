namespace VITV.Data.MigrationsGoogleAnalytcis
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class videoid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ages", "VideoId", c => c.Int());
            AddColumn("dbo.Ages", "VideoCatId", c => c.Int());
            AddColumn("dbo.Browsers", "VideoId", c => c.Int());
            AddColumn("dbo.Browsers", "VideoCatId", c => c.Int());
            AddColumn("dbo.Cities", "VideoId", c => c.Int());
            AddColumn("dbo.Cities", "VideoCatId", c => c.Int());
            AddColumn("dbo.Continents", "VideoId", c => c.Int());
            AddColumn("dbo.Continents", "VideoCatId", c => c.Int());
            AddColumn("dbo.Countries", "VideoId", c => c.Int());
            AddColumn("dbo.Countries", "VideoCatId", c => c.Int());
            AddColumn("dbo.DeviceBrandings", "VideoId", c => c.Int());
            AddColumn("dbo.DeviceBrandings", "VideoCatId", c => c.Int());
            AddColumn("dbo.DeviceCategories", "VideoId", c => c.Int());
            AddColumn("dbo.DeviceCategories", "VideoCatId", c => c.Int());
            AddColumn("dbo.DeviceInfoes", "VideoId", c => c.Int());
            AddColumn("dbo.DeviceInfoes", "VideoCatId", c => c.Int());
            AddColumn("dbo.Genders", "VideoId", c => c.Int());
            AddColumn("dbo.Genders", "VideoCatId", c => c.Int());
            AddColumn("dbo.OS", "VideoId", c => c.Int());
            AddColumn("dbo.OS", "VideoCatId", c => c.Int());
            AddColumn("dbo.Resolutions", "VideoId", c => c.Int());
            AddColumn("dbo.Resolutions", "VideoCatId", c => c.Int());
            AddColumn("dbo.SiteContents", "VideoId", c => c.Int());
            AddColumn("dbo.SiteContents", "VideoCatId", c => c.Int());
            AddColumn("dbo.SiteSearches", "VideoId", c => c.Int());
            AddColumn("dbo.SiteSearches", "VideoCatId", c => c.Int());
            AddColumn("dbo.SocialNetworkDetails", "VideoId", c => c.Int());
            AddColumn("dbo.SocialNetworkDetails", "VideoCatId", c => c.Int());
            AddColumn("dbo.SubContinents", "VideoId", c => c.Int());
            AddColumn("dbo.SubContinents", "VideoCatId", c => c.Int());
            AddColumn("dbo.UserTypes", "VideoId", c => c.Int());
            AddColumn("dbo.UserTypes", "VideoCatId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserTypes", "VideoCatId");
            DropColumn("dbo.UserTypes", "VideoId");
            DropColumn("dbo.SubContinents", "VideoCatId");
            DropColumn("dbo.SubContinents", "VideoId");
            DropColumn("dbo.SocialNetworkDetails", "VideoCatId");
            DropColumn("dbo.SocialNetworkDetails", "VideoId");
            DropColumn("dbo.SiteSearches", "VideoCatId");
            DropColumn("dbo.SiteSearches", "VideoId");
            DropColumn("dbo.SiteContents", "VideoCatId");
            DropColumn("dbo.SiteContents", "VideoId");
            DropColumn("dbo.Resolutions", "VideoCatId");
            DropColumn("dbo.Resolutions", "VideoId");
            DropColumn("dbo.OS", "VideoCatId");
            DropColumn("dbo.OS", "VideoId");
            DropColumn("dbo.Genders", "VideoCatId");
            DropColumn("dbo.Genders", "VideoId");
            DropColumn("dbo.DeviceInfoes", "VideoCatId");
            DropColumn("dbo.DeviceInfoes", "VideoId");
            DropColumn("dbo.DeviceCategories", "VideoCatId");
            DropColumn("dbo.DeviceCategories", "VideoId");
            DropColumn("dbo.DeviceBrandings", "VideoCatId");
            DropColumn("dbo.DeviceBrandings", "VideoId");
            DropColumn("dbo.Countries", "VideoCatId");
            DropColumn("dbo.Countries", "VideoId");
            DropColumn("dbo.Continents", "VideoCatId");
            DropColumn("dbo.Continents", "VideoId");
            DropColumn("dbo.Cities", "VideoCatId");
            DropColumn("dbo.Cities", "VideoId");
            DropColumn("dbo.Browsers", "VideoCatId");
            DropColumn("dbo.Browsers", "VideoId");
            DropColumn("dbo.Ages", "VideoCatId");
            DropColumn("dbo.Ages", "VideoId");
        }
    }
}
