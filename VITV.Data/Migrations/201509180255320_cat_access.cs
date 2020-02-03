namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cat_access : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoryAccesses",
                c => new
                    {
                        CategoryId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        IPViewCount = c.Int(nullable: false),
                        PageViewCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CategoryId, t.Date })
                .ForeignKey("dbo.VideoCategories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CategoryAccesses", "CategoryId", "dbo.VideoCategories");
            DropIndex("CategoryAccesses", new[] { "CategoryId" });
            DropTable("dbo.CategoryAccesses");
        }
    }
}
