namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jobequipments : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Jobs", "EquipmentId", "dbo.Equipments");
            DropForeignKey("dbo.TaskRequests", "EquipmentId", "dbo.Equipments");
            DropIndex("dbo.Jobs", new[] { "EquipmentId" });
            DropIndex("dbo.TaskRequests", new[] { "EquipmentId" });
            CreateTable(
                "dbo.JobEquipments",
                c => new
                    {
                        Job_Id = c.Int(nullable: false),
                        Equipment_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Job_Id, t.Equipment_Id })
                .ForeignKey("dbo.Jobs", t => t.Job_Id, cascadeDelete: true)
                .ForeignKey("dbo.Equipments", t => t.Equipment_Id, cascadeDelete: true)
                .Index(t => t.Job_Id)
                .Index(t => t.Equipment_Id);
            
            DropColumn("dbo.Jobs", "EquipmentId");
            DropColumn("dbo.TaskRequests", "EquipmentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaskRequests", "EquipmentId", c => c.Int());
            AddColumn("dbo.Jobs", "EquipmentId", c => c.Int());
            DropForeignKey("dbo.JobEquipments", "Equipment_Id", "dbo.Equipments");
            DropForeignKey("dbo.JobEquipments", "Job_Id", "dbo.Jobs");
            DropIndex("dbo.JobEquipments", new[] { "Equipment_Id" });
            DropIndex("dbo.JobEquipments", new[] { "Job_Id" });
            DropTable("dbo.JobEquipments");
            CreateIndex("dbo.TaskRequests", "EquipmentId");
            CreateIndex("dbo.Jobs", "EquipmentId");
            AddForeignKey("dbo.TaskRequests", "EquipmentId", "dbo.Equipments", "Id");
            AddForeignKey("dbo.Jobs", "EquipmentId", "dbo.Equipments", "Id");
        }
    }
}
