namespace Boardgames.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    using Boardgames.Utilities;

    [XmlType("Creator")]
    public class ImportCreatorsDto
    {

        [Required]
        [XmlElement("FirstName")]
        [MinLength(Constance.MinFirstNameCreator)]
        [MaxLength(Constance.MaxFirstNameCreator)]
        public string FirstName { get; set; } = null!;

        [Required]
        [XmlElement("LastName")]
        [MinLength(Constance.MinLastNameCreator)]
        [MaxLength(Constance.MaxLastNameCreator)]
        public string LastName { get; set; } = null!;

        [XmlArray("Boardgames")]
        public CreatorsBoardgamesDto[] Boardgames { get; set; } = null!;

    }
}
