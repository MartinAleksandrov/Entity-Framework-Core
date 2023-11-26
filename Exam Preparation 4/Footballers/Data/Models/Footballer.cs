namespace Footballers.Data.Models
{
    using Footballers.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Footballers.Utilities;

    public class Footballer
    {
        public Footballer()
        {
            TeamsFootballers = new HashSet<TeamFootballer>();
        }

        [Key]
        public int Id { get; set; }


        [Required]
        [MaxLength(Constance.MaxFootballerNameLenght)]
        public string Name { get; set; } = null!;


        [Required]
        public DateTime ContractStartDate { get; set; }


        [Required]
        public DateTime ContractEndDate { get; set; }


        [Required]
        public PositionType PositionType { get; set; }


        [Required]
        public BestSkillType BestSkillType { get; set; }


        [ForeignKey(nameof(Coach))]
        [Required]
        public int CoachId { get; set; }

        [Required]
        public Coach Coach { get; set; } = null!;

        public ICollection<TeamFootballer> TeamsFootballers  { get; set; }
    }
}
