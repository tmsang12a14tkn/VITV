namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sitesetting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 80),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Name);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Settings");
        }
    }
}
