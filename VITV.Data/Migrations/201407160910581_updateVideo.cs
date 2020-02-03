namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateVideo : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.ProgramScheduleDetails", "VideoId");
            AddForeignKey("dbo.ProgramScheduleDetails", "VideoId", "dbo.Videos", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProgramScheduleDetails", "FK_ProgramScheduleDetails_Videos_VideoId");
            DropIndex("dbo.ProgramScheduleDetails", new[] { "VideoId" });
        }
    }
}
