namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editReporterBioIntro : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reporters", "Biography", c => c.String());
            AlterColumn("dbo.Reporters", "Introduction", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reporters", "Introduction", c => c.String(nullable: false));
            AlterColumn("dbo.Reporters", "Biography", c => c.String(nullable: false));
        }
    }
}
