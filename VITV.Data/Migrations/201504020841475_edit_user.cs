namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit_user : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ApplicationUsers", "Email");
            DropColumn("dbo.ApplicationUsers", "EmailConfirmed");
            DropColumn("dbo.ApplicationUsers", "PhoneNumber");
            DropColumn("dbo.ApplicationUsers", "PhoneNumberConfirmed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationUsers", "PhoneNumberConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.ApplicationUsers", "PhoneNumber", c => c.String());
            AddColumn("dbo.ApplicationUsers", "EmailConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.ApplicationUsers", "Email", c => c.String());
        }
    }
}
