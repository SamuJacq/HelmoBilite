using System.ComponentModel.DataAnnotations;

namespace ProjetWeb.Models
{
    public class Address
    {
        public int? Id { get; set; }

        [MaxLength(200)]
        [Required]
        public String? Street { get; set; }

        [MaxLength(200)]
        [Required]
        public int? Number { get; set; }

        [MaxLength(200)]
        [Required]
        public String? Locality { get; set; }

        [MaxLength(200)]
        [Required]
        public int? PostalCode { get; set; }


        public Customer? Customer { get; set; }

        public Order? Order { get; set; }

    }
}
