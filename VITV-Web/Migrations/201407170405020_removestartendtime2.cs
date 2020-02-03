namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removestartendtime2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ArticleCategories", "StartTime");
            DropColumn("dbo.ArticleCategories", "EndTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ArticleCategories", "EndTime", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.ArticleCategories", "StartTime", c => c.Time(nullable: false, precision: 7));
        }
    }
}
