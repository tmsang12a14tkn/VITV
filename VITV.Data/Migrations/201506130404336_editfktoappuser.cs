namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editfktoappuser : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Videos", "UploaderId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Videos", "UploaderId");
            AddForeignKey("dbo.Videos", "UploaderId", "dbo.ApplicationUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Videos", "UploaderId", "dbo.ApplicationUsers");
            DropIndex("Videos", new[] { "UploaderId" });
            AlterColumn("dbo.Videos", "UploaderId", c => c.String());
        }
    }
}
