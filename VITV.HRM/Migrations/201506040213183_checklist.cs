namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class checklist : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Checklists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        TaskRequestId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TaskRequests", t => t.TaskRequestId, cascadeDelete: true)
                .Index(t => t.TaskRequestId);
            
            CreateTable(
                "dbo.CheckItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ChecklistId = c.Int(nullable: false),
                        Done = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Checklists", t => t.ChecklistId, cascadeDelete: true)
                .Index(t => t.ChecklistId);
            
            AddColumn("dbo.TaskRequests", "IsPrivate", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Checklists", "TaskRequestId", "dbo.TaskRequests");
            DropForeignKey("dbo.CheckItems", "ChecklistId", "dbo.Checklists");
            DropIndex("dbo.CheckItems", new[] { "ChecklistId" });
            DropIndex("dbo.Checklists", new[] { "TaskRequestId" });
            DropColumn("dbo.TaskRequests", "IsPrivate");
            DropTable("dbo.CheckItems");
            DropTable("dbo.Checklists");
        }
    }
}
