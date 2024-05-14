using MasterKinder.Models;
using Microsoft.EntityFrameworkCore;

namespace MasterKinder.Data
{
    public class AppDbContext :  DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<SurveyResponse> SurveyResponses { get; set; }

    }
}
