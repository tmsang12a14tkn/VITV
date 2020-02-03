namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editvideo : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Videos", "Content", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Videos", "Content", c => c.String(nullable: false));
        }
    }
}
