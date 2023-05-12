using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.Seed;

namespace Repository
{
    public class RepositoryContext : IdentityDbContext<User>
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new MovieSeed());
            modelBuilder.ApplyConfiguration(new CostSeed());
            modelBuilder.ApplyConfiguration(new RoleSeed());    
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Cost> Costs { get; set; }
    }
}
