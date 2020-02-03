namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changename4 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PersonalJobs", "TaskRequestId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PersonalJobs", "TaskRequestId", c => c.Int());
        }
    }
}
