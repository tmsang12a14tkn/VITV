namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class workinfoenddate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmployeeWorkInfoes", "EndDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmployeeWorkInfoes", "EndDate");
        }
    }
}
