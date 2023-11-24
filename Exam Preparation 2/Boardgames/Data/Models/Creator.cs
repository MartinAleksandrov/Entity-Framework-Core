namespace Boardgames.Data.Models
{
    using Boardgames.Utilities;
    using System.ComponentModel.DataAnnotations;

    public class Creator
    {
        public Creator()
        {
            Boardgames = new HashSet<Boardgame>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(Constance.MaxFirstNameCreator)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(Constance.MaxLastNameCreator)]
        public string LastName { get; set; } = null!;

        public ICollection<Boardgame> Boardgames { get; set; }

    }
}
