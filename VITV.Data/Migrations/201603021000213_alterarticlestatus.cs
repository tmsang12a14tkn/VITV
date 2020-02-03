namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterarticlestatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "ArticleStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "ArticleStatus");
        }
    }
}
