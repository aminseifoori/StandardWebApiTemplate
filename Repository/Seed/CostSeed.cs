using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Seed
{
    public class CostSeed : IEntityTypeConfiguration<Cost>
    {
        public void Configure(EntityTypeBuilder<Cost> builder)
        {
            builder.HasData
                (
                 new Cost
                 {
                     Id = new Guid ("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                     Amount = (decimal)12525.25,
                     Description = "Trasnportation",
                     MovieId = new Guid ("80abbca8-664d-4b20-b5de-024705497d4a")
                 },
                 new Cost
                 {
                     Id = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"),
                     Amount = (decimal)1584781.55,
                     Description = "Admin Fee",
                     MovieId = new Guid("80abbca8-664d-4b20-b5de-024705497d4a")
                 }
                );
        }
    }
}
