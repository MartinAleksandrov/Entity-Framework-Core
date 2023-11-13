using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models
{
    public class Team
    {
        public Team()
        {
            HomeGames = new HashSet<Game>();
            AwayGames = new HashSet<Game>();
            Players = new HashSet<Player>();
        }

        [Key]
        public int TeamId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required, MaxLength(100)]
        public string LogoUrl { get; set; } = null!;

        public string? Initials { get; set; }

        [Required, MaxLength(100)]
        public decimal Budget { get; set; }

        //TODO: Add navigatioanl properties 

        [ForeignKey(nameof(PrimaryKitColor))]
        public int PrimaryKitColorId { get; set; }

        public virtual Color PrimaryKitColor { get; set; } = null!;


        [ForeignKey(nameof(SecondaryKitColor))]
        public int SecondaryKitColorId { get; set; }

        public virtual Color? SecondaryKitColor { get; set; }

        [ForeignKey(nameof(Town))]
        public int TownId { get; set; }

        public virtual Town Town { get; set; } = null!;


        [InverseProperty(nameof(Game.HomeTeam))]
        public ICollection<Game> HomeGames { get; set; }


        [InverseProperty(nameof(Game.AwayTeam))]
        public ICollection<Game> AwayGames { get; set; }

        public ICollection<Player> Players { get; set; }
    }
}