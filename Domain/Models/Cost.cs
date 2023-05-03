using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Cost
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [RegularExpression("^[0-9]{1,16}(?:\\.[0-9]{1,2})?$")]
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        [Required]
        [ForeignKey("MovieId")]
        public Guid MovieId { get; set; }
        public Movie? Movie { get; set; }
    }
}
