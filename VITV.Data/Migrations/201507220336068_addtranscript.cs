namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtranscript : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VideoTranscripts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HourSeek = c.Int(nullable: false),
                        MinuteSeek = c.Int(nullable: false),
                        SecondSeek = c.Int(nullable: false),
                        ConvertedToSeconds = c.Int(nullable: false),
                        Title = c.String(),
                        Content = c.String(),
                        VideoId = c.Int(nullable: false),
                        UserEditedId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.UserEditedId)
                .ForeignKey("dbo.Videos", t => t.VideoId, cascadeDelete: true)
                .Index(t => t.VideoId)
                .Index(t => t.UserEditedId);
            
            CreateTable(
                "dbo.VideoTranscriptEmployees",
                c => new
                    {
                        VideoTranscript_Id = c.Int(nullable: false),
                        Employee_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VideoTranscript_Id, t.Employee_Id })
                .ForeignKey("dbo.VideoTranscripts", t => t.VideoTranscript_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.Employee_Id, cascadeDelete: true)
                .Index(t => t.VideoTranscript_Id)
                .Index(t => t.Employee_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VideoTranscripts", "VideoId", "dbo.Videos");
            DropForeignKey("dbo.VideoTranscripts", "UserEditedId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.VideoTranscriptEmployees", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.VideoTranscriptEmployees", "VideoTranscript_Id", "dbo.VideoTranscripts");
            DropIndex("VideoTranscriptEmployees", new[] { "Employee_Id" });
            DropIndex("VideoTranscriptEmployees", new[] { "VideoTranscript_Id" });
            DropIndex("VideoTranscripts", new[] { "UserEditedId" });
            DropIndex("VideoTranscripts", new[] { "VideoId" });
            DropTable("dbo.VideoTranscriptEmployees");
            DropTable("dbo.VideoTranscripts");
        }
    }
}
