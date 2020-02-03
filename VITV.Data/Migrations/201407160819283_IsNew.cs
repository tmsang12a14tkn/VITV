namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsNew : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProgramScheduleDetails", "IsNew", c => c.String());
            AddColumn("dbo.ProgramSchedules", "IsNew", c => c.Boolean(nullable: false));
            DropColumn("dbo.ProgramSchedules", "IsShown");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProgramSchedules", "IsShown", c => c.Boolean(nullable: false));
            DropColumn("dbo.ProgramSchedules", "IsNew");
            DropColumn("dbo.ProgramScheduleDetails", "IsNew");
        }
    }
}
