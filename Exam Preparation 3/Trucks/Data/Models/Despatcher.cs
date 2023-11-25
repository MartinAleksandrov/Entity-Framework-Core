namespace Trucks.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Trucks.Utilities;

    public class Despatcher
    {
        public Despatcher()
        {
            Trucks = new HashSet<Truck>();
        }

        [Key]
        public int Id { get; set; }


        [Required]
        [MaxLength(Constance.MaxDespatcherNameLenght)]
        public string Name { get; set; } = null!;


        public string? Position { get; set; }

        public ICollection<Truck> Trucks { get; set; }
    }
}
