namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class accountlocked : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationUsers", "Locked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationUsers", "Locked");
        }
    }
}
