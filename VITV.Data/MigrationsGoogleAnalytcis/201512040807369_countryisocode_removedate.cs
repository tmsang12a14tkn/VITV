namespace VITV.Data.MigrationsGoogleAnalytcis
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class countryisocode_removedate : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.CountryIsoCodes");
            AddPrimaryKey("dbo.CountryIsoCodes", new[] { "Name", "PageUrl" });
            DropColumn("dbo.CountryIsoCodes", "CreateDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CountryIsoCodes", "CreateDate", c => c.DateTime(nullable: false));
            DropPrimaryKey("dbo.CountryIsoCodes");
            AddPrimaryKey("dbo.CountryIsoCodes", new[] { "CreateDate", "Name", "PageUrl" });
        }
    }
}
