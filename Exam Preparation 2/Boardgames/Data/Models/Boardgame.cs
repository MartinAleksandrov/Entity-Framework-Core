namespace Boardgames.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Boardgames.Data.Models.Enums;
    using Boardgames.Utilities;

    public class Boardgame
    {

        public Boardgame()
        {
            BoardgamesSellers = new HashSet<BoardgameSeller>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(Constance.MaxNameLenght)]
        public string Name { get; set; } = null!;


        [Required]
        [MaxLength(Constance.MaxRating)]
        public double Rating  { get; set; }

        [Required]
        [MaxLength (Constance.MaxYearPublished)]
        public int YearPublished  { get; set; }


        [Required]
        [MaxLength(5)]
        public CategoryType CategoryType { get; set; }

        [Required]
        public string Mechanics { get; set; } = null!;

        [ForeignKey(nameof(Creator))]
        [Required]
        public int CreatorId  { get; set; }

        public Creator Creator { get; set; } = null!;

        public ICollection<BoardgameSeller> BoardgamesSellers  { get; set; }

    }
}
