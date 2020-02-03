namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editvideo3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "UploaderId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Videos", "UploaderId");
        }
    }
}
