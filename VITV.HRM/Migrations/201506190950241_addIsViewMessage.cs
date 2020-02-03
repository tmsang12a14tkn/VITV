namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIsViewMessage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "IsView", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "IsView");
        }
    }
}
