namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class privateproject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "IsPrivate", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "IsPrivate");
        }
    }
}
