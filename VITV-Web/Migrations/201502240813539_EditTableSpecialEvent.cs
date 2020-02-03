namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditTableSpecialEvent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SpecialEvents", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SpecialEvents", "EndDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.SpecialEvents", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SpecialEvents", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.SpecialEvents", "EndDate");
            DropColumn("dbo.SpecialEvents", "StartDate");
        }
    }
}
