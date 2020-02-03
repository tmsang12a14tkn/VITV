namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uniquetitlecat_2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ArticleCategories", "UniqueTitle", c => c.String(maxLength: 450));
            AlterColumn("dbo.VideoCategories", "UniqueTitle", c => c.String(maxLength: 450));
            CreateIndex("dbo.ArticleCategories", "UniqueTitle", unique: true, name: "TitleUniqueIndex");
            CreateIndex("dbo.VideoCategories", "UniqueTitle", unique: true, name: "TitleUniqueIndex");
        }
        
        public override void Down()
        {
            DropIndex("dbo.VideoCategories", "TitleUniqueIndex");
            DropIndex("dbo.ArticleCategories", "TitleUniqueIndex");
            AlterColumn("dbo.VideoCategories", "UniqueTitle", c => c.String());
            AlterColumn("dbo.ArticleCategories", "UniqueTitle", c => c.String());
        }
    }
}
