namespace Footballers.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Footballers.Utilities;

    public class Coach
    {
        public Coach()
        {
            Footballers = new HashSet<Footballer>();
        }

        [Key]
        public int Id { get; set; }


        [Required]
        [MaxLength(Constance.MaxCoachNameLenght)]
        public string Name { get; set; } = null!;


        [Required]
        public string Nationality { get; set; } = null!;

        public ICollection<Footballer> Footballers { get; set; }
    }
}
