namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editDelLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeletionLogs", "Note", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DeletionLogs", "Note");
        }
    }
}
