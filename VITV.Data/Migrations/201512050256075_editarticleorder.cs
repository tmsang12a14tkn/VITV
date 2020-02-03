namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editarticleorder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "Order");
        }
    }
}
