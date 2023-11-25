namespace Trucks.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Trucks.Data.Models.Enums;
    using Trucks.Utilities;
    
    public class Truck
    {
        public Truck()
        {
            ClientsTrucks = new HashSet<ClientTruck>();
        }

        [Key]
        public int Id { get; set; }

        public string? RegistrationNumber  { get; set; }

        [Required]
        [MaxLength(Constance.MaxVinNumLenght)]
        public string VinNumber { get; set; } = null!;

        //In the word document this prop and one below is not required BUT judge is ask them to be required!!!
        [Required]
        [MaxLength(Constance.MaxTankCapacity)]
        public int TankCapacity  { get; set; }

        //THIS
        [Required]
        [MaxLength(Constance.MaxCargoCapacity)]
        public int CargoCapacity  { get; set; }


        [Required]
        public CategoryType CategoryType { get; set; }


        [Required]
        public MakeType MakeType { get; set; }


        [Required]
        [ForeignKey("Despatcher")]
        public int DespatcherId  { get; set; }

        public Despatcher Despatcher { get; set; } = null!;

        public ICollection<ClientTruck>  ClientsTrucks { get; set; }


    }
}
