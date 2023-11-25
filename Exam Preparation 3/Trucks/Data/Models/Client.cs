namespace Trucks.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Trucks.Utilities;

    public class Client
    {
        public Client()
        {
            ClientsTrucks = new HashSet<ClientTruck>();
        }

        [Key]
        public int Id { get; set; }


        [Required]
        [MaxLength(Constance.MaxClientNameLenght)]
        public string Name { get; set; } = null!;


        [Required]
        [MaxLength(Constance.MaxClientNationalityLenght)]
        public string Nationality  { get; set; } = null!;


        [Required]
        public string Type { get; set; } = null!;


        public ICollection<ClientTruck> ClientsTrucks  { get; set; }
    }
}
