namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class introcategory : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VideoCategories", "CategoryIntroId", "dbo.CategoryIntroes");
            DropIndex("VideoCategories", new[] { "CategoryIntroId" });
            AddColumn("dbo.CategoryIntroes", "VideoCategoryId", c => c.Int(nullable: false, defaultValue: 1));
            AddColumn("dbo.CategoryIntroes", "Selected", c => c.Boolean(nullable: false));
            AddColumn("dbo.TVCCompanies", "Logo", c => c.String());
            CreateIndex("dbo.CategoryIntroes", "VideoCategoryId");
            AddForeignKey("dbo.CategoryIntroes", "VideoCategoryId", "dbo.VideoCategories", "Id", cascadeDelete: true);
            DropColumn("dbo.VideoCategories", "CategoryIntroId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VideoCategories", "CategoryIntroId", c => c.Int());
            DropForeignKey("dbo.CategoryIntroes", "VideoCategoryId", "dbo.VideoCategories");
            DropIndex("CategoryIntroes", new[] { "VideoCategoryId" });
            DropColumn("dbo.TVCCompanies", "Logo");
            DropColumn("dbo.CategoryIntroes", "Selected");
            DropColumn("dbo.CategoryIntroes", "VideoCategoryId");
            CreateIndex("dbo.VideoCategories", "CategoryIntroId");
            AddForeignKey("dbo.VideoCategories", "CategoryIntroId", "dbo.CategoryIntroes", "Id");
        }
    }
}
