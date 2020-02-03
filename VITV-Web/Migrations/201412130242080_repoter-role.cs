namespace VITV_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class repoterrole : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleReporters",
                c => new
                    {
                        Role_Id = c.Int(nullable: false),
                        Reporter_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.Reporter_Id })
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .ForeignKey("dbo.Reporters", t => t.Reporter_Id, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.Reporter_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RoleReporters", "FK_RoleReporters_Reporters_Reporter_Id");
            DropForeignKey("dbo.RoleReporters", "FK_RoleReporters_Roles_Role_Id");
            DropIndex("dbo.RoleReporters", new[] { "Reporter_Id" });
            DropIndex("dbo.RoleReporters", new[] { "Role_Id" });
            DropTable("dbo.RoleReporters");
            DropTable("dbo.Roles");
        }
    }
}
