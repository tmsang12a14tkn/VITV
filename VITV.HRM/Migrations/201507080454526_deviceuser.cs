namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deviceuser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeviceUsers",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        DeviceId = c.String(nullable: false, maxLength: 128),
                        DeviceType = c.Int(nullable: false),
                        DeviceToken = c.String(),
                        LoginToken = c.String(),
                        ExpiredTime = c.String(),
                        Logged = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.DeviceId, t.DeviceType })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeviceUsers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.DeviceUsers", new[] { "UserId" });
            DropTable("dbo.DeviceUsers");
        }
    }
}
