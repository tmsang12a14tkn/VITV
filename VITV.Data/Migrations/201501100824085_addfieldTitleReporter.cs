namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfieldTitleReporter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reporters", "UniqueTitle", c => c.String(maxLength: 450));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reporters", "UniqueTitle");
        }
    }
}
