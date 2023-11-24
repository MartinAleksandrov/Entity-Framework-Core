namespace Boardgames.DataProcessor.ImportDto
{
    using Boardgames.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using Boardgames.Utilities;

    using System.Xml.Serialization;

    [XmlType("Boardgame")]
    public class CreatorsBoardgamesDto
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(Constance.MinNameLenght)]
        [MaxLength(Constance.MaxNameLenght)]
        public string Name { get; set; } = null!;


        [XmlElement("Rating")]
        [Range(Constance.MinRating, Constance.MaxRating)]
        [Required]
        public double Rating { get; set; }


        [XmlElement("YearPublished")]
        [Range(Constance.MinYearPublished,Constance.MaxYearPublished)]
        [Required]
        public int YearPublished { get; set; }


        [XmlElement("CategoryType")]
        [Required]
        [Range(0, 4)]
        public int CategoryType { get; set; } 

        [XmlElement("Mechanics")]
        [Required]
        public string Mechanics { get; set; } = null!;
    }
}
