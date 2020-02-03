namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addextra : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RecruitExtraInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        RecruitRule = c.String(),
                        RecruitForm = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RecruitExtraInfoes");
        }
    }
}
