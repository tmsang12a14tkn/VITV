namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editrecruit5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recruits", "Thumbnail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Recruits", "Thumbnail");
        }
    }
}
