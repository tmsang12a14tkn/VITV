namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class videopiority : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "VideoPiority", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Videos", "VideoPiority");
        }
    }
}
