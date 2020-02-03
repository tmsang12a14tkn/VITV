namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_IsSpecial : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Videos", "IsSpecial");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Videos", "IsSpecial", c => c.Boolean(nullable: false));
        }
    }
}
