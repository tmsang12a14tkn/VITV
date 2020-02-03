namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editpa_rm_colBrowser : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PageAccesses", "BrowserName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PageAccesses", "BrowserName", c => c.String());
        }
    }
}
