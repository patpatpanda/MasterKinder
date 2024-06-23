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

        
        public DbSet<School> Schools { get; set; }
        public DbSet<Response> Responses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<School>()
                .HasMany(s => s.Responses)
                .WithOne(r => r.School)
                .HasForeignKey(r => r.SchoolId);
        }

    }

}
