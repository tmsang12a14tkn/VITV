namespace VITV.Data.MigrationsGoogleAnalytcis
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class countryisocode_fullname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CountryIsoCodes", "FullName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CountryIsoCodes", "FullName");
        }
    }
}
