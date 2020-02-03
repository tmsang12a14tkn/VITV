namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hastranscipt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VideoCategories", "HasTranscript", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VideoCategories", "HasTranscript");
        }
    }
}
