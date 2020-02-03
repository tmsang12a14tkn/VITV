namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changename2 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Checklists", newName: "JobLists");
            RenameColumn(table: "dbo.Jobs", name: "ChecklistId", newName: "JobListId");
            RenameIndex(table: "dbo.Jobs", name: "IX_ChecklistId", newName: "IX_JobListId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Jobs", name: "IX_JobListId", newName: "IX_ChecklistId");
            RenameColumn(table: "dbo.Jobs", name: "JobListId", newName: "ChecklistId");
            RenameTable(name: "dbo.JobLists", newName: "Checklists");
        }
    }
}
