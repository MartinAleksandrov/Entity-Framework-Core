namespace Medicines.DataProcessor.ImportDtos
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    using Medicines.Utilities;

    [XmlType("Pharmacy")]
    public class ImportPharmacyDto
    {
        [Required]
        [XmlAttribute("non-stop")]
        public string NonStop { get; set; } = null!;

        [Required]
        [XmlElement("Name")]
        [MinLength(2)]
        [MaxLength(50)]
        public string Name { get; set; } = null!;


        [XmlElement("PhoneNumber")]
        [RegularExpression(GlobalConstance.PhoneNumberRegex)]
        [MinLength(14)]
        [MaxLength(14)]
        public string PhoneNumber { get; set; } = null!;


        [XmlArray("Medicines")]
        public ImportMedicinesDto[] Medicines { get; set; }

    }
}
