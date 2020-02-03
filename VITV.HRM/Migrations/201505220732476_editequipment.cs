namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editequipment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Equipments", "EquipPicture", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Equipments", "EquipPicture");
        }
    }
}
