namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRecruit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RecruitCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        IsRecruiting = c.Boolean(nullable: false),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Recruits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        JobPosition = c.String(nullable: false),
                        Amount = c.String(),
                        CategoryId = c.Int(nullable: false),
                        WorkingPlace = c.String(nullable: false),
                        JobFeature = c.String(),
                        Description = c.String(),
                        Requirement = c.String(),
                        Experience = c.String(),
                        Degree = c.String(),
                        Gender = c.String(),
                        JobType = c.String(),
                        Salary = c.String(),
                        Probation = c.String(),
                        Remuneration = c.String(),
                        Resume = c.String(),
                        DateExpire = c.String(),
                        Submission = c.String(),
                        ContactInfo = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RecruitCategories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Recruits", "FK_Recruits_RecruitCategories_CategoryId");
            DropIndex("dbo.Recruits", new[] { "CategoryId" });
            DropTable("dbo.Recruits");
            DropTable("dbo.RecruitCategories");
        }
    }
}
