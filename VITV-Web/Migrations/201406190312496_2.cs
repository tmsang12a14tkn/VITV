namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Products", "Content", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Content", c => c.String());
            AlterColumn("dbo.Products", "Title", c => c.String());
        }
    }
}
