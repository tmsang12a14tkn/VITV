using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using VITV_Web.Models;

namespace VITV_Web.DAL
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

        public DbSet<Video> Videos { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<ArticleCategory> ArticleCategories { get; set; }

        public DbSet<Reporter> Reporters { get; set; }

        public DbSet<Info> Infoes { get; set; }

        public DbSet<Keyword> Keywords { get; set; }


        public DbSet<Contact> Contacts { get; set; }

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

        public DbSet<PositionGroup> PositionGroups { get; set; }

        public DbSet<Role> ReporterRoles { get; set; }

        public DbSet<SpecialEvent> SpecialEvents { get; set; }

        public DbSet<Holiday> Holidays { get; set; }

        public DbSet<Bank> Banks { get; set; }

        public DbSet<Term> Terms { get; set; }

        public DbSet<InterestRate> InterestRates { get; set; }

        public DbSet<Page> Pages { get; set; }
        public DbSet<PageAccess> PageAccesses { get; set; }
    }
}