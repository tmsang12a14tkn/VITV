namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class videoduration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "Duration", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Videos", "Duration");
        }
    }
}
