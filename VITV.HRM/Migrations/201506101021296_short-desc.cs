namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shortdesc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "ShortDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jobs", "ShortDescription");
        }
    }
}
