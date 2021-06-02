using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FragranceWebsite.ViewModels
{
    public class Sale
    {
        public int SaleId { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public byte Quantity { get; set; }

        public decimal FinalPrice { get; set; }

        [Required]
        public int FragranceId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public FragranceVM Fragrance { get; set; }

        public CustomerVM Customer { get; set; }
    }
}
