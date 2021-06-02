using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Entities
{
    public class Sale
    {
        [Key]
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

        [ForeignKey("FragranceId")]
        public Fragrance Fragrance { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

    }
}
