namespace VITV.Data.MigrationsGoogleAnalytcis
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class socialurl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SocialNetworkDetails", "Url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SocialNetworkDetails", "Url");
        }
    }
}
