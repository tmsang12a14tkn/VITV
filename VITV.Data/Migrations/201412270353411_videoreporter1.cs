namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class videoreporter1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Videos", "FK_Videos_Reporters_ReporterId");
            DropIndex("dbo.Videos", new[] { "ReporterId" });
            RenameColumn(table: "dbo.Videos", name: "ReporterId", newName: "Reporter_Id");
            AlterColumn("dbo.Videos", "Reporter_Id", c => c.Int());
            CreateIndex("dbo.Videos", "Reporter_Id");
            AddForeignKey("dbo.Videos", "Reporter_Id", "dbo.Reporters", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Videos", "FK_Videos_Reporters_Reporter_Id");
            DropIndex("dbo.Videos", new[] { "Reporter_Id" });
            AlterColumn("dbo.Videos", "Reporter_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Videos", name: "Reporter_Id", newName: "ReporterId");
            CreateIndex("dbo.Videos", "ReporterId");
            AddForeignKey("dbo.Videos", "ReporterId", "dbo.Reporters", "Id", cascadeDelete: true);
        }
    }
}
