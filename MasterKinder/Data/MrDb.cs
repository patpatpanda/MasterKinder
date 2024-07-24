using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MasterKinder.Models;
using KinderReader.Models;
using Microsoft.AspNetCore.Identity;

namespace MasterKinder.Data
{
    public class MrDb : IdentityDbContext<IdentityUser>
    {
        public MrDb(DbContextOptions<MrDb> options)
            : base(options)
        {
        }

        public DbSet<Forskolan> Forskolans { get; set; }
        public DbSet<KontaktInfo> kontaktInfos { get; set; }
        public DbSet<PdfData> PdfData { get; set; }
        public DbSet<PdfAllData> PdfAllData { get; set; }

        public DbSet<PostBlog> PostBlogs { get; set; }
        public DbSet<SurveyResult> SurveyResults { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Forskolan>()
                .HasMany(f => f.Kontakter)
                .WithOne(k => k.Forskolan)
                .HasForeignKey(k => k.ForskolanId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(p => new { p.LoginProvider, p.ProviderKey });
        }
    }
}
