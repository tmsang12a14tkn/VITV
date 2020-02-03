namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changename1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CheckItems", newName: "Jobs");
            RenameTable(name: "dbo.Tasks", newName: "PersonalJobs");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.PersonalJobs", newName: "Tasks");
            RenameTable(name: "dbo.Jobs", newName: "CheckItems");
        }
    }
}
