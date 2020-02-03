namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editrecruit1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Recruits", "Title");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Recruits", "Title", c => c.String(nullable: false));
        }
    }
}
