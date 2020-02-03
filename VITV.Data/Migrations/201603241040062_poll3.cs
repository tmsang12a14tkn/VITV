namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class poll3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PollAnswers", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.PollQuestions", "Displayed", c => c.Boolean(nullable: false));
            AddColumn("dbo.PollQuestions", "ViewTotal", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PollQuestions", "ViewTotal");
            DropColumn("dbo.PollQuestions", "Displayed");
            DropColumn("dbo.PollAnswers", "Order");
        }
    }
}
