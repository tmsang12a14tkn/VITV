namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editvideo2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Videos", "UploaderId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Videos", "UploaderId", c => c.Int(nullable: false));
        }
    }
}
