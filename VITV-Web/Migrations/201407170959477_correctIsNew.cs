namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class correctIsNew : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProgramScheduleDetails", "IsNew", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProgramScheduleDetails", "IsNew", c => c.String());
        }
    }
}
