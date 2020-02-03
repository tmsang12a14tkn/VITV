namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uniquetitle_video : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "UniqueTitle", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Videos", "UniqueTitle");
        }
    }
}
