namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dayoff : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Dayoffs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Begin = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Dayoffs");
        }
    }
}
