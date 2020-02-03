namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editvideoskipintro : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "HourSkip", c => c.Int(nullable: false));
            AddColumn("dbo.Videos", "MinuteSkip", c => c.Int(nullable: false));
            AddColumn("dbo.Videos", "SecondSkip", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Videos", "SecondSkip");
            DropColumn("dbo.Videos", "MinuteSkip");
            DropColumn("dbo.Videos", "HourSkip");
        }
    }
}
