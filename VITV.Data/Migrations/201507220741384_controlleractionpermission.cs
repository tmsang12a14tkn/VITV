namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class controlleractionpermission : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ControllerActionPermissions",
                c => new
                    {
                        ControllerActionId = c.Int(nullable: false),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ControllerActionId, t.RoleId })
                .ForeignKey("dbo.ControllerActions", t => t.ControllerActionId, cascadeDelete: true)
                .ForeignKey("dbo.IdentityRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.ControllerActionId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ControllerActions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ControllerName = c.String(),
                        ActionName = c.String(),
                        Site = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ControllerActionPermissions", "RoleId", "dbo.IdentityRoles");
            DropForeignKey("dbo.ControllerActionPermissions", "ControllerActionId", "dbo.ControllerActions");
            DropIndex("ControllerActionPermissions", new[] { "RoleId" });
            DropIndex("ControllerActionPermissions", new[] { "ControllerActionId" });
            DropTable("dbo.ControllerActions");
            DropTable("dbo.ControllerActionPermissions");
        }
    }
}
