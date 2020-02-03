namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editrecruit : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.RecruitCategories", "Order");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RecruitCategories", "Order", c => c.Int(nullable: false));
        }
    }
}
