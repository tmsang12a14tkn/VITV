namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editReporterIsShow : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reporters", "IsShow", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reporters", "IsShow");
        }
    }
}
