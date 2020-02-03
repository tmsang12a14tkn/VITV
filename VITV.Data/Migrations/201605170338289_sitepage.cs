namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sitepage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SitePages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Slug = c.String(nullable: false, maxLength: 450),
                        MainContent = c.String(nullable: false),
                        CreatedUserId = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.CreatedUserId, cascadeDelete: true)
                .Index(t => t.Slug, unique: true)
                .Index(t => t.CreatedUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SitePages", "CreatedUserId", "dbo.ApplicationUsers");
            DropIndex("SitePages", new[] { "CreatedUserId" });
            DropIndex("SitePages", new[] { "Slug" });
            DropTable("dbo.SitePages");
        }
    }
}
