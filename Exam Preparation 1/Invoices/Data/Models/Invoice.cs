namespace Invoices.Data.Models
{
    using Invoices.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(1_500_000_000)]
        public int Number  { get; set; }

        [Required]
        public DateTime IssueDate  { get; set; }

        [Required]
        public DateTime DueDate  { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public CurrencyType CurrencyType { get; set; }

        [Required]
        [ForeignKey(nameof(Client))]
        public int ClientId  { get; set; }

        [Required]
        public Client Client { get; set; } = null!;


    }
}
