namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_articlecat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArticleCategories", "ParentId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ArticleCategories", "ParentId");
        }
    }
}
