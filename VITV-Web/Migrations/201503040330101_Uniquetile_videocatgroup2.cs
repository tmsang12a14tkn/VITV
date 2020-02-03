namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Uniquetile_videocatgroup2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VideoCatGroups", "UniqueTitle", c => c.String(maxLength: 450));
            CreateIndex("dbo.VideoCatGroups", "UniqueTitle", unique: true, name: "TitleUniqueIndex");
        }
        
        public override void Down()
        {
            DropIndex("dbo.VideoCatGroups", "TitleUniqueIndex");
            AlterColumn("dbo.VideoCatGroups", "UniqueTitle", c => c.String());
        }
    }
}
