using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestFeatures
{
    public class CostParameters : RequestParameters
    {
        public CostParameters()
        {
            OrderBy = "Amount";
        }
        public decimal MinAmount { get; set; } = 0;
        public decimal MaxAmount { get; set; } = (decimal)9999999999999999.99m;

        public bool ValidAmountRange => MaxAmount > MinAmount;
        public string? SearchTerm { get; set; }
    }
}
