namespace Artillery.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Artillery.Utilities;

    public class Manufacturer
    {
        public Manufacturer()
        {
            Guns = new HashSet<Gun>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstance.MaxManufacturerNameLenght)]
        public string ManufacturerName { get; set; } = null!;

        [Required]
        [MaxLength(GlobalConstance.MaxFoundedLenght)]
        public string Founded { get; set; } = null!;

        public ICollection<Gun> Guns { get; set; }
    }
}
