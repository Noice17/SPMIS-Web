using Microsoft.EntityFrameworkCore;
using SPMIS_Web.Models.Entities;

namespace SPMIS_Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            

        }
        public DbSet<StrategyMap> StrategyMaps { get; set; }
        public DbSet<Objective> Objectives { get; set; }
        public DbSet<ObjectiveType> ObjectiveTypes { get; set; }
    }
}
