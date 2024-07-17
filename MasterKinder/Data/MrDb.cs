using KinderReader.Models;
using MasterKinder.Models;
using Microsoft.EntityFrameworkCore;

namespace MasterKinder.Data
{
    public class MrDb : DbContext
    {
        public MrDb(DbContextOptions<MrDb> options)
            : base(options)
        {
        }

        public DbSet<Forskolan> Forskolans { get; set; }
        public DbSet<KontaktInfo> kontaktInfos { get; set; }
        public DbSet<PdfData> PdfData { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Forskolan>()
                .HasMany(f => f.Kontakter)
                .WithOne(k => k.Forskolan)
                .HasForeignKey(k => k.ForskolanId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed some test data
            modelBuilder.Entity<Category>().HasData(new Category { Id = 1, Name = "General" });
            modelBuilder.Entity<BlogPost>().HasData(new BlogPost
            {
                Id = 1,
                Title = "First Blog Post",
                Content = "This is the content of the first blog post.",
                PublishedDate = DateTime.Now,
                CategoryId = 1,
                Slug = "first-blog-post"
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
