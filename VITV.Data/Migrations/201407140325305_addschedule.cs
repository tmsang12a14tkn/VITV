namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addschedule : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VideoCatGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.VideoCategories", "GroupId", c => c.Int());
            CreateIndex("dbo.VideoCategories", "GroupId");
            AddForeignKey("dbo.VideoCategories", "GroupId", "dbo.VideoCatGroups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VideoCategories", "FK_VideoCategories_VideoCatGroups_GroupId");
            DropIndex("dbo.VideoCategories", new[] { "GroupId" });
            DropColumn("dbo.VideoCategories", "GroupId");
            DropTable("dbo.VideoCatGroups");
        }
    }
}
