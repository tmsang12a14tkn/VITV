namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetvc2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("VideoTVCs", new[] { "ProductgroupId" });
            CreateIndex("dbo.VideoTVCs", "ProductGroupId");
        }
        
        public override void Down()
        {
            DropIndex("VideoTVCs", new[] { "ProductGroupId" });
            CreateIndex("dbo.VideoTVCs", "ProductgroupId");
        }
    }
}
