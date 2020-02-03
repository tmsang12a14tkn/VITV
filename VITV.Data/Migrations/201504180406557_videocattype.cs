namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class videocattype : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VideoCatTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.VideoCategories", "ParentId", c => c.Int());
            AddColumn("dbo.VideoCategories", "TypeId", c => c.Int());
            CreateIndex("dbo.VideoCategories", "ParentId");
            CreateIndex("dbo.VideoCategories", "TypeId");
            AddForeignKey("dbo.VideoCategories", "ParentId", "dbo.VideoCategories", "Id");
            AddForeignKey("dbo.VideoCategories", "TypeId", "dbo.VideoCatTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VideoCategories", "TypeId", "dbo.VideoCatTypes");
            DropForeignKey("dbo.VideoCategories", "ParentId", "dbo.VideoCategories");
            DropIndex("VideoCategories", new[] { "TypeId" });
            DropIndex("VideoCategories", new[] { "ParentId" });
            DropColumn("dbo.VideoCategories", "TypeId");
            DropColumn("dbo.VideoCategories", "ParentId");
            DropTable("dbo.VideoCatTypes");
        }
    }
}
