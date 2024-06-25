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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Forskolan>()
                .HasMany(f => f.Kontakter)
                .WithOne(k => k.Forskolan)
                .HasForeignKey(k => k.ForskolanId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
