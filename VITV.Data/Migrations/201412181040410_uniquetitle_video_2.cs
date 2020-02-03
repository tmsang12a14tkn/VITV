namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uniquetitle_video_2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Videos", "UniqueTitle", c => c.String(maxLength: 450));
            CreateIndex("dbo.Videos", "UniqueTitle", unique: true, name: "TitleUniqueIndex");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Videos", "TitleUniqueIndex");
            AlterColumn("dbo.Videos", "UniqueTitle", c => c.String());
        }
    }
}
