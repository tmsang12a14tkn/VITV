namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateartcat : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProgramScheduleDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VideoCategoryId = c.Int(nullable: false),
                        VideoId = c.Int(),
                        DateTime = c.DateTime(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VideoCategories", t => t.VideoCategoryId, cascadeDelete: true)
                .Index(t => t.VideoCategoryId);
            
            AddColumn("dbo.ArticleCategories", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProgramScheduleDetails", "FK_ProgramScheduleDetails_VideoCategories_VideoCategoryId");
            DropIndex("dbo.ProgramScheduleDetails", new[] { "VideoCategoryId" });
            DropColumn("dbo.ArticleCategories", "Order");
            DropTable("dbo.ProgramScheduleDetails");
        }
    }
}
