namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class poll2 : DbMigration
    {
        public override void Up()
        {
       
          
            AddColumn("dbo.PollQuestions", "Name", c => c.String());
            RenameColumn("dbo.PollAnswers", "Order", "AnswerId");
            RenameColumn("dbo.PollQuestions", "Title", "Question");
            RenameColumn("dbo.PollUserAnswers", "Order", "AnswerId");
         
        }
        
        public override void Down()
        {
            RenameColumn("dbo.PollAnswers", "AnswerId", "Order");
            RenameColumn("dbo.PollUserAnswers", "AnswerId", "Order");
            RenameColumn("dbo.PollQuestions", "Question", "Title");
            DropColumn("dbo.PollQuestions", "Name");
        }
    }
}
