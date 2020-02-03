namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class departmentbranchid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Departments", "BranchId", c => c.Int());
            CreateIndex("dbo.Departments", "BranchId");
            AddForeignKey("dbo.Departments", "BranchId", "dbo.Branches", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Departments", "BranchId", "dbo.Branches");
            DropIndex("dbo.Departments", new[] { "BranchId" });
            DropColumn("dbo.Departments", "BranchId");
        }
    }
}
