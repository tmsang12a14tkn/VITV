namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removestartendtime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VideoCategories", "TimeFrame", c => c.String());
            DropColumn("dbo.VideoCategories", "StartTime");
            DropColumn("dbo.VideoCategories", "EndTime");
            DropTable("dbo.Cities");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.VideoCategories", "EndTime", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.VideoCategories", "StartTime", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.VideoCategories", "TimeFrame");
        }
    }
}
