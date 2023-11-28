namespace Artillery.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Artillery.Utilities;

    [XmlType("Shell")]
    public class ImportShellDto
    {
        [XmlElement("ShellWeight")]
        [Required]
        [Range(GlobalConstance.MinShellWeightLenght,GlobalConstance.MaxShellWeightLenght)]
        public double ShellWeight { get; set; }


        [XmlElement("Caliber")]
        [MinLength(GlobalConstance.MinCaliberSize)]
        [MaxLength(GlobalConstance.MaxCaliberSize)]
        [Required]
        public string Caliber { get; set; } = null!;
    }
}
