namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edittaskrequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskRequests", "EquipmentId", c => c.Int());
            AddColumn("dbo.TaskRequests", "RequestFrom", c => c.DateTime());
            AddColumn("dbo.TaskRequests", "RequestTo", c => c.DateTime());
            CreateIndex("dbo.TaskRequests", "EquipmentId");
            AddForeignKey("dbo.TaskRequests", "EquipmentId", "dbo.Equipments", "Id");
            DropColumn("dbo.TaskRequests", "DeviceId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaskRequests", "DeviceId", c => c.Int());
            DropForeignKey("dbo.TaskRequests", "EquipmentId", "dbo.Equipments");
            DropIndex("dbo.TaskRequests", new[] { "EquipmentId" });
            DropColumn("dbo.TaskRequests", "RequestTo");
            DropColumn("dbo.TaskRequests", "RequestFrom");
            DropColumn("dbo.TaskRequests", "EquipmentId");
        }
    }
}
