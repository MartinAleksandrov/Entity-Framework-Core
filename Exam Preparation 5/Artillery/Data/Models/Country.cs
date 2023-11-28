namespace Artillery.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Artillery.Utilities; 

    public class Country
    {
        public Country()
        {
            CountriesGuns = new HashSet<CountryGun>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstance.MaxCountryNameLenght)]
        public string CountryName  { get; set; } = null!;


        [Required]
        [MaxLength(GlobalConstance.MaxArmySize)]
        public int ArmySize  { get; set; }

        public ICollection<CountryGun> CountriesGuns  { get; set; }
    }
}
