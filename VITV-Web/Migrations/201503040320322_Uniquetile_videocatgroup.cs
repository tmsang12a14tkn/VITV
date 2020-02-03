namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Uniquetile_videocatgroup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VideoCatGroups", "UniqueTitle", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VideoCatGroups", "UniqueTitle");
        }
    }
}
