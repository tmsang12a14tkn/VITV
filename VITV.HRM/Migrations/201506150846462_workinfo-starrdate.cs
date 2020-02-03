namespace VITV.HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class workinfostarrdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EmployeeWorkInfoes", "StartDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EmployeeWorkInfoes", "StartDate", c => c.DateTime(nullable: false));
        }
    }
}
