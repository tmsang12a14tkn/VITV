namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editvideo_size : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "VideoWidth", c => c.Int(nullable: false));
            AddColumn("dbo.Videos", "VideoHeight", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Videos", "VideoHeight");
            DropColumn("dbo.Videos", "VideoWidth");
        }
    }
}
