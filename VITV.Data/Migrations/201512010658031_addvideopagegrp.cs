namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addvideopagegrp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PageGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Order = c.Int(nullable: false),
                        UniqueTitle = c.String(maxLength: 450),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UniqueTitle, unique: true, name: "TitleUniqueIndex");
            
            AddColumn("dbo.VideoCategories", "PageGroupId", c => c.Int());
            CreateIndex("dbo.VideoCategories", "PageGroupId");
            AddForeignKey("dbo.VideoCategories", "PageGroupId", "dbo.PageGroups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VideoCategories", "PageGroupId", "dbo.PageGroups");
            DropIndex("PageGroups", "TitleUniqueIndex");
            DropIndex("VideoCategories", new[] { "PageGroupId" });
            DropColumn("dbo.VideoCategories", "PageGroupId");
            DropTable("dbo.PageGroups");
        }
    }
}
