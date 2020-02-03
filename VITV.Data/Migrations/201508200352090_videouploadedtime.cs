namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class videouploadedtime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "UploadedTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Videos", "UploadedTime");
        }
    }
}
