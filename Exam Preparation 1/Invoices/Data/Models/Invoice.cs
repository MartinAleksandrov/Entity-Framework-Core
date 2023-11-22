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
        [MaxLength(1500000000)]
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
        public int ClientId  { get; set; }

        [ForeignKey(nameof(ClientId))]
        public Client Client { get; set; } = null!;


    }
}
