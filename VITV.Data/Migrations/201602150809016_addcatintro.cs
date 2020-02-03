namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcatintro : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoryIntroes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Thumbnail = c.String(nullable: false),
                        Url = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.VideoCategories", "CategoryIntroId", c => c.Int());
            CreateIndex("dbo.VideoCategories", "CategoryIntroId");
            AddForeignKey("dbo.VideoCategories", "CategoryIntroId", "dbo.CategoryIntroes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VideoCategories", "CategoryIntroId", "dbo.CategoryIntroes");
            DropIndex("VideoCategories", new[] { "CategoryIntroId" });
            DropColumn("dbo.VideoCategories", "CategoryIntroId");
            DropTable("dbo.CategoryIntroes");
        }
    }
}
