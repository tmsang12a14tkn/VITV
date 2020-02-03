namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editbank : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Banks", "IssueDate", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Banks", "IssueDate", c => c.DateTime());
        }
    }
}
