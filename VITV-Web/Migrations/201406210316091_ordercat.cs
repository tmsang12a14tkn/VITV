namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ordercat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VideoCategories", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VideoCategories", "Order");
        }
    }
}
