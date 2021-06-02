using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FragranceWebsite.ViewModels
{
    public class CustomerVM
    {
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(10)]
        public string Phone { get; set; }

        public enum AddressOption { Home, Office }

        [Required]
        public AddressOption AddressType { get; set; }

        [Required]
        [MaxLength(255)]
        public string Address { get; set; }
    }
}
