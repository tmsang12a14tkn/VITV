namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class videodisplaytime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "DisplayTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Videos", "DisplayTime");
        }
    }
}
