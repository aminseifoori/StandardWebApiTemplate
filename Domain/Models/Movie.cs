using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Movie
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(150)]
        public string? Name { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public ICollection<Cost>? Costs { get; set; }

    }
}
