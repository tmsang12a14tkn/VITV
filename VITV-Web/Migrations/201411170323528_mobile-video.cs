namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mobilevideo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "MobileThumbnail", c => c.String());/**/
        }
        
        public override void Down()
        {
            DropColumn("dbo.Videos", "MobileThumbnail");
        }
    }
}
