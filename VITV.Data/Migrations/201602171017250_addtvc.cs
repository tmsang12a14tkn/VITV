namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtvc : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VideoTVCs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Thumbnail = c.String(nullable: false),
                        Url = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.VideoTVCs");
        }
    }
}
