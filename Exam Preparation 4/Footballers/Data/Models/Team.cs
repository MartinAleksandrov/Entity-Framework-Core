namespace Footballers.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Footballers.Utilities;

    public class Team
    {
        public Team()
        {
            TeamsFootballers = new HashSet<TeamFootballer>();
        }

        [Key]
        public int Id { get; set; }


        [Required]
        [MaxLength(Constance.MaxTeamNameLenght)]
        public string Name { get; set; } = null!;


        [Required]
        [MaxLength(Constance.MaxNationalityLenght)]
        public string Nationality { get; set; } = null!;


        [Required]
        public int Trophies { get; set; }

        public ICollection<TeamFootballer> TeamsFootballers { get; set; }
    }
}
