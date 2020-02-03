namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mobilecat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VideoCategories", "IntroductionBonus", c => c.String());/**/
        }
        
        public override void Down()
        {
            DropColumn("dbo.VideoCategories", "IntroductionBonus");
        }
    }
}
