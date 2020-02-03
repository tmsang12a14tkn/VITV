namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editcontact : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContactStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Contacts", "CreatedDatetime", c => c.DateTime());
            AddColumn("dbo.Contacts", "StatusId", c => c.Int());
            AddColumn("dbo.Contacts", "IsRead", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Contacts", "StatusId");
            AddForeignKey("dbo.Contacts", "StatusId", "dbo.ContactStatus", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contacts", "StatusId", "dbo.ContactStatus");
            DropIndex("Contacts", new[] { "StatusId" });
            DropColumn("dbo.Contacts", "IsRead");
            DropColumn("dbo.Contacts", "StatusId");
            DropColumn("dbo.Contacts", "CreatedDatetime");
            DropTable("dbo.ContactStatus");
        }
    }
}
