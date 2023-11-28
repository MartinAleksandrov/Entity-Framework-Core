namespace Artillery.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CountryGun
    {
        [Required]
        [ForeignKey(nameof(Country))]
        public int CountryId  { get; set; }

        [Required]
        public Country Country { get; set; } = null!;


        [Required]
        [ForeignKey(nameof(Gun))]
        public int GunId  { get; set; }

        [Required]
        public Gun Gun { get; set; } = null!;
    }
}
