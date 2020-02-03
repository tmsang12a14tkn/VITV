namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editrecruit4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recruits", "IsRecruiting", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Recruits", "IsRecruiting");
        }
    }
}
