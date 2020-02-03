namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editrecruit2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Recruits", "CityId", c => c.Int(nullable: true));
            CreateIndex("dbo.Recruits", "CityId");
            AddForeignKey("dbo.Recruits", "CityId", "dbo.Cities", "Id", cascadeDelete: true);
            DropColumn("dbo.Recruits", "WorkingPlace");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Recruits", "WorkingPlace", c => c.String(nullable: false));
            DropForeignKey("dbo.Recruits", "FK_Recruits_Cities_CityId");
            DropIndex("dbo.Recruits", new[] { "CityId" });
            DropColumn("dbo.Recruits", "CityId");
            DropTable("dbo.Cities");
        }
    }
}
