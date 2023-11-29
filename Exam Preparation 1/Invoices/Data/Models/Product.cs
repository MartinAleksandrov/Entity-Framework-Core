namespace Invoices.Data.Models
{
    using Invoices.Data.Models.Enums;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        public Product()
        {
            ProductsClients = new HashSet<ProductClient>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = null!;


        [Required]
        [MaxLength(1000)]
        public decimal Price { get; set; }

        [Required]
        public CategoryType CategoryType { get; set; }

        public ICollection<ProductClient> ProductsClients { get; set; }
    }
}
