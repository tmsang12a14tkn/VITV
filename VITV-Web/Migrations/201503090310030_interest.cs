namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class interest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Banks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Thumbnail = c.String(nullable: false),
                        IssueDate = c.DateTime(),
                        UpdateTime = c.DateTime(),
                        CharterCapital = c.Double(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InterestRates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BankId = c.Int(nullable: false),
                        TermId = c.Int(nullable: false),
                        PercentValue = c.Double(nullable: false),
                        TypeValue = c.Int(nullable: false),
                        TargetValue = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Banks", t => t.BankId, cascadeDelete: true)
                .ForeignKey("dbo.Terms", t => t.TermId, cascadeDelete: true)
                .Index(t => t.BankId)
                .Index(t => t.TermId);
            
            CreateTable(
                "dbo.Terms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        MonthValue = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InterestRates", "FK_InterestRates_Terms_TermId");
            DropForeignKey("dbo.InterestRates", "FK_InterestRates_Banks_BankId");
            DropIndex("dbo.InterestRates", new[] { "TermId" });
            DropIndex("dbo.InterestRates", new[] { "BankId" });
            DropTable("dbo.Terms");
            DropTable("dbo.InterestRates");
            DropTable("dbo.Banks");
        }
    }
}
