using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Repositories.DTOs
{
   public class SaleUpdateDTO
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
    }
}
