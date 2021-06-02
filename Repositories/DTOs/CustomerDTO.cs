using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static Data.Entities.Customer;

namespace Repositories.DTOs
{
    public class CustomerDTO
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

        [Required]
        public AddressOption AddressType { get; set; }

        [Required]
        [MaxLength(255)]
        public string Address { get; set; }
    }
}
