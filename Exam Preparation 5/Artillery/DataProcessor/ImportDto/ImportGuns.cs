namespace Artillery.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using Artillery.Utilities;
    using Artillery.Data.Models.Enums;

    public class ImportGuns
    {
        [JsonProperty("ManufacturerId")]
        [Required]
        public int ManufacturerId { get; set; }


        [JsonProperty("GunWeight")]
        [Required]
        [Range(GlobalConstance.MinGunWeight,GlobalConstance.MaxGunWeight)]
        public int GunWeight { get; set; }


        [JsonProperty("BarrelLength")]
        [Required]
        [Range(GlobalConstance.MinBarrelLength, GlobalConstance.MaxBarrelLength)]
        public double BarrelLength { get; set; }


        [JsonProperty("NumberBuild")]
        public int? NumberBuild { get; set; }


        [JsonProperty("Range")]
        [Required]
        [Range(GlobalConstance.MinGunRange,GlobalConstance.MaxGunRange)]
        public int Range { get; set; }


        [JsonProperty("GunType")]
        [Range(0,5)]
        public string? GunType { get; set; }


        [JsonProperty("ShellId")]
        [Required]
        public int ShellId { get; set; }


        [JsonProperty("Countries")]
        public ImportCountriesIds[]? Countries { get; set; }
    }
}
