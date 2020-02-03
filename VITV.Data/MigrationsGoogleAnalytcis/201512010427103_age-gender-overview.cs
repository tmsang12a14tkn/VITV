namespace VITV.Data.MigrationsGoogleAnalytcis
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class agegenderoverview : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AgeOverviews",
                c => new
                    {
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
                    })
                .PrimaryKey(t => new { t.Name, t.PageUrl });
            
            CreateTable(
                "dbo.GenderOverviews",
                c => new
                    {
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
                    })
                .PrimaryKey(t => new { t.Name, t.PageUrl });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GenderOverviews");
            DropTable("dbo.AgeOverviews");
        }
    }
}
