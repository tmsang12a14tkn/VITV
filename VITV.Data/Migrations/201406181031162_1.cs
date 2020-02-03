namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArticleCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        ProfilePhoto = c.String(nullable: false),
                        StartTime = c.Time(nullable: false, precision: 7),
                        EndTime = c.Time(nullable: false, precision: 7),
                        ShowInMenu = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        ShortBrief = c.String(nullable: false),
                        ArticleContent = c.String(nullable: false),
                        PublishedTime = c.DateTime(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        ReporterId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ArticleCategories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Reporters", t => t.ReporterId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.ReporterId);
            
            CreateTable(
                "dbo.Keywords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Videos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Thumbnail = c.String(nullable: false),
                        Url = c.String(nullable: false),
                        YoutubeUrl = c.String(),
                        Title = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        PublishedTime = c.DateTime(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        ReporterId = c.Int(nullable: false),
                        IsHot = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VideoCategories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Reporters", t => t.ReporterId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.ReporterId);
            
            CreateTable(
                "dbo.VideoCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Introduction = c.String(nullable: false),
                        ProfilePhoto = c.String(nullable: false),
                        StartTime = c.Time(nullable: false, precision: 7),
                        EndTime = c.Time(nullable: false, precision: 7),
                        ShowInMenu = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VideoCategoryHighlights",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(nullable: false),
                        Photo = c.String(nullable: false),
                        VideoCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VideoCategories", t => t.VideoCategoryId, cascadeDelete: true)
                .Index(t => t.VideoCategoryId);
            
            CreateTable(
                "dbo.Reporters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Phone = c.String(),
                        Email = c.String(),
                        ProfilePicture = c.String(nullable: false),
                        Biography = c.String(nullable: false),
                        Introduction = c.String(nullable: false),
                        Position = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Title = c.String(nullable: false),
                        ContactContent = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Infoes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 160),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProgramCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Programs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProgramCategories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.IdentityRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserRoles",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.IdentityRoles", t => t.IdentityRole_Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.IdentityRole_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.IdentityUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.VideoKeywords",
                c => new
                    {
                        VideoId = c.Int(nullable: false),
                        KeywordId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VideoId, t.KeywordId })
                .ForeignKey("dbo.Videos", t => t.VideoId, cascadeDelete: true)
                .ForeignKey("dbo.Keywords", t => t.KeywordId, cascadeDelete: true)
                .Index(t => t.VideoId)
                .Index(t => t.KeywordId);
            
            CreateTable(
                "dbo.ArticleKeywords",
                c => new
                    {
                        ArticleId = c.Int(nullable: false),
                        KeywordId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ArticleId, t.KeywordId })
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.Keywords", t => t.KeywordId, cascadeDelete: true)
                .Index(t => t.ArticleId)
                .Index(t => t.KeywordId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentityUserRoles", "FK_IdentityUserRoles_ApplicationUsers_ApplicationUser_Id");
            DropForeignKey("dbo.IdentityUserLogins", "FK_IdentityUserLogins_ApplicationUsers_ApplicationUser_Id");
            DropForeignKey("dbo.IdentityUserClaims", "FK_IdentityUserClaims_ApplicationUsers_ApplicationUser_Id");
            DropForeignKey("dbo.IdentityUserRoles", "FK_IdentityUserRoles_IdentityRoles_IdentityRole_Id");
            DropForeignKey("dbo.Programs", "FK_Programs_ProgramCategories_CategoryId");
            DropForeignKey("dbo.Articles", "FK_Articles_Reporters_ReporterId");
            DropForeignKey("dbo.ArticleKeywords", "FK_ArticleKeywords_Keywords_KeywordId");
            DropForeignKey("dbo.ArticleKeywords", "FK_ArticleKeywords_Articles_ArticleId");
            DropForeignKey("dbo.Videos", "FK_Videos_Reporters_ReporterId");
            DropForeignKey("dbo.VideoKeywords", "FK_VideoKeywords_Keywords_KeywordId");
            DropForeignKey("dbo.VideoKeywords", "FK_VideoKeywords_Videos_VideoId");
            DropForeignKey("dbo.Videos", "FK_Videos_VideoCategories_CategoryId");
            DropForeignKey("dbo.VideoCategoryHighlights", "FK_VideoCategoryHighlights_VideoCategories_VideoCategoryId");
            DropForeignKey("dbo.Articles", "FK_Articles_ArticleCategories_CategoryId");
            DropIndex("dbo.ArticleKeywords", new[] { "KeywordId" });
            DropIndex("dbo.ArticleKeywords", new[] { "ArticleId" });
            DropIndex("dbo.VideoKeywords", new[] { "KeywordId" });
            DropIndex("dbo.VideoKeywords", new[] { "VideoId" });
            DropIndex("dbo.IdentityUserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserClaims", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.Programs", new[] { "CategoryId" });
            DropIndex("dbo.VideoCategoryHighlights", new[] { "VideoCategoryId" });
            DropIndex("dbo.Videos", new[] { "ReporterId" });
            DropIndex("dbo.Videos", new[] { "CategoryId" });
            DropIndex("dbo.Articles", new[] { "ReporterId" });
            DropIndex("dbo.Articles", new[] { "CategoryId" });
            DropTable("dbo.ArticleKeywords");
            DropTable("dbo.VideoKeywords");
            DropTable("dbo.IdentityUserLogins");
            DropTable("dbo.IdentityUserClaims");
            DropTable("dbo.ApplicationUsers");
            DropTable("dbo.IdentityUserRoles");
            DropTable("dbo.IdentityRoles");
            DropTable("dbo.Programs");
            DropTable("dbo.ProgramCategories");
            DropTable("dbo.Products");
            DropTable("dbo.Infoes");
            DropTable("dbo.Contacts");
            DropTable("dbo.Reporters");
            DropTable("dbo.VideoCategoryHighlights");
            DropTable("dbo.VideoCategories");
            DropTable("dbo.Videos");
            DropTable("dbo.Keywords");
            DropTable("dbo.Articles");
            DropTable("dbo.ArticleCategories");
        }
    }
}
