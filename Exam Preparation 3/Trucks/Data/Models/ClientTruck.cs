namespace Trucks.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ClientTruck
    {
        [Required]
        [ForeignKey("Client")]
        public int ClientId { get; set; }
        
        [Required]
        public Client Client { get; set; } = null!;


        [Required]
        [ForeignKey("Truck")]
        public int TruckId { get; set; }

        [Required]
        public Truck Truck { get; set; } = null!;
    }
}
