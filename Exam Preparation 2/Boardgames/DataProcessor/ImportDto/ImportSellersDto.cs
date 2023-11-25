namespace Boardgames.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using Boardgames.Utilities;
    public class ImportSellersDto
    {
        [Required]
        [MinLength(Constance.MinSellerName)]
        [MaxLength(Constance.MaxSellerName)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(Constance.MinAddressLenght)]
        [MaxLength(Constance.MaxAddressLenght)]
        public string Address { get; set; } = null!;

        [Required]
        public string Country { get; set; } = null!;


        [Required]
        [RegularExpression(Constance.WebSiteRegex)]
        public string Website { get; set; } = null!;

        [Required]
        public int[] Boardgames { get; set; }

    }
}
