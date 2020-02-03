namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatevideoaccess1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pages", "Title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pages", "Title");
        }
    }
}
