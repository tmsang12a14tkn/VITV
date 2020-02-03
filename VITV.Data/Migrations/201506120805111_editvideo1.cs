namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editvideo1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "UploaderId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Videos", "UploaderId");
        }
    }
}
