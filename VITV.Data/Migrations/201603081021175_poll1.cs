namespace VITV.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class poll1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PollAnswers",
                c => new
                    {
                        QuestionId = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        Answer = c.String(),
                        Image = c.String(),
                    })
                .PrimaryKey(t => new { t.QuestionId, t.Order })
                .ForeignKey("dbo.PollQuestions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.PollQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VideoId = c.Int(nullable: false),
                        FromDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        Done = c.Boolean(nullable: false),
                        Published = c.Boolean(nullable: false),
                        Title = c.String(),
                        MultipleChoice = c.Boolean(nullable: false),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Videos", t => t.VideoId, cascadeDelete: true)
                .Index(t => t.VideoId);
            
            CreateTable(
                "dbo.PollUserAnswers",
                c => new
                    {
                        QuestionId = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        IP = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.QuestionId, t.Order, t.IP })
                .ForeignKey("dbo.PollQuestions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PollAnswers", "QuestionId", "dbo.PollQuestions");
            DropForeignKey("dbo.PollQuestions", "VideoId", "dbo.Videos");
            DropForeignKey("dbo.PollUserAnswers", "QuestionId", "dbo.PollQuestions");
            DropIndex("PollUserAnswers", new[] { "QuestionId" });
            DropIndex("PollQuestions", new[] { "VideoId" });
            DropIndex("PollAnswers", new[] { "QuestionId" });
            DropTable("dbo.PollUserAnswers");
            DropTable("dbo.PollQuestions");
            DropTable("dbo.PollAnswers");
        }
    }
}
