namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class delLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeletionLogs",
                c => new
                    {
                        DeletionTime = c.DateTime(nullable: false),
                        VideoId = c.Int(nullable: false),
                        VideoName = c.String(),
                        CategoryName = c.String(),
                        DisplayTime = c.DateTime(nullable: false),
                        CreationUser = c.String(),
                        DeletionUser = c.String(),
                    })
                .PrimaryKey(t => t.DeletionTime);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DeletionLogs");
        }
    }
}
