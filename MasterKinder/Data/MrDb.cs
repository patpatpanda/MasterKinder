using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MasterKinder.Models;
using KinderReader.Models;
using Microsoft.AspNetCore.Identity;

namespace MasterKinder.Data
{
    public class MrDb : IdentityDbContext<ApplicationUser> // Använd ApplicationUser istället för IdentityUser
    {
        public MrDb(DbContextOptions<MrDb> options) : base(options) { }

        public DbSet<Forskolan> Forskolans { get; set; }
        public DbSet<KontaktInfo> kontaktInfos { get; set; }

        public DbSet<PostBlog> PostBlogs { get; set; }
        public DbSet<Malibu> Malibus { get; set; }
        public DbSet<Question> Questions { get; set; }

        public DbSet<SurveyResponse> SurveyResponses { get; set; }
        public DbSet<SurveyResponses_2021> SurveyResponses2021 { get; set; }
        public DbSet<SurveyResponses_2022> SurveyResponses2022 { get; set; }
        public DbSet<SurveyResponses_2023> SurveyResponses2023 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Definiera relationen mellan förskolor och kontakter
            modelBuilder.Entity<Forskolan>()
                .HasMany(f => f.Kontakter)
                .WithOne(k => k.Forskolan)
                .HasForeignKey(k => k.ForskolanId)
                .OnDelete(DeleteBehavior.Cascade);

            // Definiera en composite key för IdentityUserLogin
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(p => new { p.LoginProvider, p.ProviderKey });
        }
    }
}
