namespace Boardgames.DataProcessor.ExportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Boardgame")]
    public class ExportBoardGames
    {
        [XmlElement("BoardgameName")]
        [Required]
        public string BoardgameName { get; set; } = null!;


        [XmlElement("BoardgameYearPublished")]
        [Required]
        public int BoardgameYearPublished { get; set; }
    }
}
