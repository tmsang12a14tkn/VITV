namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uniquetitle_article : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "UniqueTitle", c => c.String(maxLength: 450));
            CreateIndex("dbo.Articles", "UniqueTitle", unique: true, name: "TitleUniqueIndex");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Articles", "TitleUniqueIndex");
            DropColumn("dbo.Articles", "UniqueTitle");
        }
    }
}
