namespace Footballers.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using Footballers.Utilities;

    public class ImportTeamsDto
    {
        [Required]
        [JsonProperty("Name")]
        [MinLength(Constance.MinTeamNameLenght)]
        [MaxLength(Constance.MaxTeamNameLenght)]
        [RegularExpression(Constance.RegexTeamName)]
        public string Name { get; set; } = null!;


        [Required]
        [JsonProperty("Nationality")]
        [MinLength(Constance.MinFootballerNameLenght)]
        [MaxLength(Constance.MaxNationalityLenght)]
        public string Nationality { get; set; } = null!;


        [Required]
        [JsonProperty("Trophies")]
        public int Trophies { get; set; }


        [JsonProperty("Footballers")]
        public int[] Footballers { get; set; }
    }
}
