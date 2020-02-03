namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateschdule1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProgramSchedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VideoCategoryId = c.Int(nullable: false),
                        DayOfWeek = c.Int(nullable: false),
                        Time = c.Time(nullable: false, precision: 7),
                        IsShown = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VideoCategories", t => t.VideoCategoryId, cascadeDelete: true)
                .Index(t => t.VideoCategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProgramSchedules", "FK_ProgramSchedules_VideoCategories_VideoCategoryId");
            DropIndex("dbo.ProgramSchedules", new[] { "VideoCategoryId" });
            DropTable("dbo.ProgramSchedules");
        }
    }
}
