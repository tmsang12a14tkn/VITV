namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editrecruit3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Recruits", "FK_Recruits_Cities_CityId");
            DropIndex("dbo.Recruits", new[] { "CityId" });
            AlterColumn("dbo.Recruits", "CityId", c => c.Int(nullable: false));
            CreateIndex("dbo.Recruits", "CityId");
            AddForeignKey("dbo.Recruits", "CityId", "dbo.Cities", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Recruits", "FK_Recruits_Cities_CityId");
            DropIndex("dbo.Recruits", new[] { "CityId" });
            AlterColumn("dbo.Recruits", "CityId", c => c.Int());
            CreateIndex("dbo.Recruits", "CityId");
            AddForeignKey("dbo.Recruits", "CityId", "dbo.Cities", "Id");
        }
    }
}
