namespace Footballers.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class TeamFootballer
    {
        [ForeignKey(nameof(Team))]
        [Required]
        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;


        [ForeignKey(nameof(Footballer))]
        [Required]
        public int FootballerId  { get; set; }
        public Footballer Footballer { get; set; } = null!;
    }
}
