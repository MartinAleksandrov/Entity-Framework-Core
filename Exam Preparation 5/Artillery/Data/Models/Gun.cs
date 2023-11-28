namespace Artillery.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Artillery.Data.Models.Enums;
    using Artillery.Utilities;

    public class Gun
    {
        public Gun()
        {
            CountriesGuns = new HashSet<CountryGun>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Manufacturer))]
        public int ManufacturerId { get; set; }

        [Required]
        public Manufacturer Manufacturer { get; set; } = null!;


        [Required]
        [MaxLength(GlobalConstance.MaxGunWeight)]
        public int GunWeight { get; set; }


        [Required]
        [MaxLength(GlobalConstance.MaxBarrelLength)]
        public double BarrelLength  { get; set;}

        //ATTENTION If some property is not required you must initial to tell EF core that the prop can accept null(whith this Symbol -> ?)
        public int? NumberBuild { get; set; }

        [Required]
        [MaxLength(GlobalConstance.MaxGunRange)]
        public int Range { get; set; }

        [Required]
        [MaxLength(5)]
        public GunType GunType { get; set; }

        [Required]
        [ForeignKey(nameof(Shell))]
        public int ShellId  { get; set; }

        [Required]
        public Shell Shell { get; set; } = null!;

        public ICollection<CountryGun> CountriesGuns  { get; set; }

    }
}
