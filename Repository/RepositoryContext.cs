using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Seed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MovieSeed());
            modelBuilder.ApplyConfiguration(new CostSeed());
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Cost> Costs { get; set; }
    }
}
