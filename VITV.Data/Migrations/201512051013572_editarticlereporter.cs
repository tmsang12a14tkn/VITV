namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editarticlereporter : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Articles", "ReporterId", "dbo.Employees");
            DropIndex("Articles", new[] { "ReporterId" });
            CreateTable(
                "dbo.EmployeeArticles",
                c => new
                    {
                        Employee_Id = c.Int(nullable: false),
                        Article_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Employee_Id, t.Article_Id })
                .ForeignKey("dbo.Employees", t => t.Employee_Id, cascadeDelete: true)
                .ForeignKey("dbo.Articles", t => t.Article_Id, cascadeDelete: true)
                .Index(t => t.Employee_Id)
                .Index(t => t.Article_Id);

            DropColumn("dbo.Articles", "ReporterId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articles", "ReporterId", c => c.Int(nullable: false));
            DropForeignKey("dbo.EmployeeArticles", "Article_Id", "dbo.Articles");
            DropForeignKey("dbo.EmployeeArticles", "Employee_Id", "dbo.Employees");
            DropIndex("EmployeeArticles", new[] { "Article_Id" });
            DropIndex("EmployeeArticles", new[] { "Employee_Id" });
            DropTable("dbo.EmployeeArticles");
            CreateIndex("dbo.Articles", "ReporterId");
            AddForeignKey("dbo.Articles", "ReporterId", "dbo.Employees", "Id", cascadeDelete: true);
        }
    }
}
