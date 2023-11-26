namespace Trucks.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using Trucks.Utilities;

    public class ImportClientsDto
    {
        [JsonProperty("Name")]
        [Required]
        [MinLength(Constance.MinClientNameLenght)]
        [MaxLength(Constance.MaxClientNameLenght)]
        public string Name { get; set; } = null!;


        [JsonProperty("Nationality")]
        [Required]
        [MinLength(Constance.MinClientNationalityLenght)]
        [MaxLength(Constance.MaxClientNationalityLenght)]
        public string Nationality  { get; set; } = null!;

        [JsonProperty("Type")]
        [Required]
        public string Type { get; set; } = null!;

        public int[] Trucks { get; set; } = null!;
    }
}
