using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Entities
{
   public class Fragrance
    {
        [Key]
        public int FragranceId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Brand { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        public short ReleaseYear { get; set; }

        [Required]
        [MaxLength(40)]
        public string Perfumer { get; set; }

        public enum FragranceOption { Designer, PrivateCollection, Niche}

        [Required]
        public FragranceOption Type { get; set; }

        [MaxLength(255)]
        public string MainNotes { get; set; }

        [Required]
        public decimal Price { get; set; }

        public byte[] Picture { get; set; }


    }
}
