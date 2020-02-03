namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class videoreporter2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReporterVideos",
                c => new
                    {
                        Reporter_Id = c.Int(nullable: false),
                        Video_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Reporter_Id, t.Video_Id })
                .ForeignKey("dbo.Reporters", t => t.Reporter_Id, cascadeDelete: true)
                .ForeignKey("dbo.Videos", t => t.Video_Id, cascadeDelete: true)
                .Index(t => t.Reporter_Id)
                .Index(t => t.Video_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReporterVideos", "FK_ReporterVideos_Videos_Video_Id");
            DropForeignKey("dbo.ReporterVideos", "FK_ReporterVideos_Reporters_Reporter_Id");
            DropIndex("dbo.ReporterVideos", new[] { "Video_Id" });
            DropIndex("dbo.ReporterVideos", new[] { "Reporter_Id" });
            DropTable("dbo.ReporterVideos");
        }
    }
}
