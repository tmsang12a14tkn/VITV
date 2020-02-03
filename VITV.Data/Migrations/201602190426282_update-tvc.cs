namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetvc : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TVCCompanyGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TVCProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TVCCompanies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.TVCCompanies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TVCProductGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.VideoTVCs", "CompanyGroupId", c => c.Int());
            AddColumn("dbo.VideoTVCs", "ProductgroupId", c => c.Int());
            AddColumn("dbo.VideoTVCs", "ProductId", c => c.Int());
            AddColumn("dbo.VideoTVCs", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.VideoTVCs", "DisplayCount", c => c.Int(nullable: false));
            CreateIndex("dbo.VideoTVCs", "CompanyGroupId");
            CreateIndex("dbo.VideoTVCs", "ProductgroupId");
            CreateIndex("dbo.VideoTVCs", "ProductId");
            AddForeignKey("dbo.VideoTVCs", "CompanyGroupId", "dbo.TVCCompanyGroups", "Id");
            AddForeignKey("dbo.VideoTVCs", "ProductId", "dbo.TVCProducts", "Id");
            AddForeignKey("dbo.VideoTVCs", "ProductgroupId", "dbo.TVCProductGroups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VideoTVCs", "ProductgroupId", "dbo.TVCProductGroups");
            DropForeignKey("dbo.VideoTVCs", "ProductId", "dbo.TVCProducts");
            DropForeignKey("dbo.TVCProducts", "CompanyId", "dbo.TVCCompanies");
            DropForeignKey("dbo.VideoTVCs", "CompanyGroupId", "dbo.TVCCompanyGroups");
            DropIndex("TVCProducts", new[] { "CompanyId" });
            DropIndex("VideoTVCs", new[] { "ProductId" });
            DropIndex("VideoTVCs", new[] { "ProductgroupId" });
            DropIndex("VideoTVCs", new[] { "CompanyGroupId" });
            DropColumn("dbo.VideoTVCs", "DisplayCount");
            DropColumn("dbo.VideoTVCs", "Version");
            DropColumn("dbo.VideoTVCs", "ProductId");
            DropColumn("dbo.VideoTVCs", "ProductgroupId");
            DropColumn("dbo.VideoTVCs", "CompanyGroupId");
            DropTable("dbo.TVCProductGroups");
            DropTable("dbo.TVCCompanies");
            DropTable("dbo.TVCProducts");
            DropTable("dbo.TVCCompanyGroups");
        }
    }
}
