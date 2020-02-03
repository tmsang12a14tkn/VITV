using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using VITV.Data.Models;

namespace VITV.Data.DAL
{
    public class VITVContext : IdentityDbContext<ApplicationUser>
    {
        public VITVContext()
            : base("VITVContext")
        {

        }

        public static VITVContext Create()
        {
            return new VITVContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            //modelBuilder.Entity<IdentityUserClaim>().ToTable("IdentityUserClaim");
            //modelBuilder.Entity<IdentityUserLogin>().ToTable("IdentityUserLogin");
            //modelBuilder.Entity<IdentityRole>().ToTable("IdentityRole");
            //modelBuilder.Entity<ApplicationUser>().ToTable("ApplicationUser");
            //modelBuilder.Entity<IdentityUserRole>().ToTable("IdentityUserRole");

            modelBuilder.Entity<Video>().
                HasMany(v => v.Keywords).
                WithMany(k => k.Videos).
                Map(
                    m =>
                    {
                        m.MapLeftKey("VideoId");
                        m.MapRightKey("KeywordId");
                        m.ToTable("VideoKeywords");
                    }
                );
            modelBuilder.Entity<Article>().
               HasMany(v => v.Keywords).
               WithMany(k => k.Articles).
               Map(
                   m =>
                   {
                       m.MapLeftKey("ArticleId");
                       m.MapRightKey("KeywordId");
                       m.ToTable("ArticleKeywords");
                   }
               );
        }

        public DbSet<Program> Programs { get; set; }

        public DbSet<ProgramCategory> ProgramCategories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<VideoCategory> VideoCategories { get; set; }

        public DbSet<VideoCatType> VideoCatTypes { get; set; }

        public DbSet<Video> Videos { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<ArticleCategory> ArticleCategories { get; set; }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeePersonalInfo> EmployeePersonalInfos { get; set; }
        public DbSet<EmployeeWorkInfo> EmployeeWorkInfos { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Agency> Agencies { get; set; }

        public DbSet<Info> Infoes { get; set; }

        public DbSet<Keyword> Keywords { get; set; }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactStatus> ContactStatuses { get; set; }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<VNDExchangeRate> VNDExchangeRates { get; set; }

        public DbSet<VideoCatGroup> VideoCatGroups { get; set; }

        public DbSet<ProgramSchedule> ProgramSchedules { get; set; }

        public DbSet<ProgramScheduleDetail> ProgramScheduleDetails { get; set; }

        public DbSet<Recruit> Recruits { get; set; }

        public DbSet<RecruitCategory> RecruitCategories { get; set; }

        public DbSet<RecruitExtraInfo> RecruitExtraInfoes { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Partner> Partners { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<PositionLevel> PositionLevels { get; set; }

        public DbSet<Role> ReporterRoles { get; set; }

        public DbSet<SpecialEvent> SpecialEvents { get; set; }

        public DbSet<Holiday> Holidays { get; set; }

        public DbSet<Bank> Banks { get; set; }

        public DbSet<Term> Terms { get; set; }

        public DbSet<InterestRate> InterestRates { get; set; }

        public DbSet<Page> Pages { get; set; }
        public DbSet<PageAccess> PageAccesses { get; set; }
        public DbSet<CategoryAccess> CategoryAccesses { get; set; }
        public DbSet<ArticleCategoryAccess> ArticleCategoryAccesses { get; set; }
        public DbSet<CategoryIntro> CategoryIntroes { get; set; }
        public DbSet<VideoTVC> VideoTVCs { get; set; }
        public DbSet<VideoCategorySponsor> VideoCategorySponsors { get; set; }
        public DbSet<ControllerActionPermission> ControllerActionPermissions { get; set; }
        public DbSet<ControllerAction> ControllerActions { get; set; }

        public DbSet<VideoTranscript> VideoTranscripts { get; set; }

        public DbSet<VideoRate> VideoRates { get; set; }

        public DbSet<PageGroup> PageGroups { get; set; }

        public DbSet<ArticleSeries> ArticleSeries { get; set; }

        public DbSet<ArticleHighlightAll> ArticleHighlightAlls { get; set; }

        public DbSet<ArticleHighlightCat> ArticleHighlightCats { get; set; }
        public DbSet<ArticleRevision> ArticleRevisions { get; set; }
        //public DbSet<ArticleReview> ArticleReviews { get; set; }
        public DbSet<DeletionLog> DeletionLogs { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<TVCCompany> TVCCompanies { get; set; }
        public DbSet<TVCProduct> TVCProducts { get; set; }
        public DbSet<TVCProductGroup> TVCProductGroups { get; set; }
        public DbSet<TVCCompanyGroup> TVCCompanyGroups { get; set; }

        public DbSet<PollQuestion> PollQuestions { get; set; }
        public DbSet<PollAnswer> PollAnswers { get; set; }
        public DbSet<PollUserAnswer> PollUserAnswers { get; set; }

        public DbSet<Subtitle> Subtitles { get; set; }

        public DbSet<VideoAccess> VideoAccesses { get; set; }
        public DbSet<ArticleAccess> ArticleAccesses { get; set; }
        public DbSet<ArticleComment> ArticleComments { get; set; }
        public DbSet<ArticleCommentReply> ArticleCommentReplies { get; set; }

        public DbSet<SitePage> SitePages { get; set; }
    }
}