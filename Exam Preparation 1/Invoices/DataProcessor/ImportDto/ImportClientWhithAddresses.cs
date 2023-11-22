﻿namespace Invoices.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Address")]
    public class ImportClientWhithAddresses
    {
        [XmlElement("StreetName")]
        [Required]
        [MaxLength(20)]
        [MinLength(10)]
        public string StreetName { get; set; } = null!;


        [XmlElement("StreetNumber")]
        [Required]
        public int StreetNumber { get; set; }


        [XmlElement("PostCode")]
        [Required]
        public string PostCode { get; set; } = null!;


        [XmlElement("City")]
        [Required]
        [MaxLength(5)]
        [MinLength(15)]
        public string City { get; set; } = null!;


        [XmlElement("Country")]
        [Required]
        [MaxLength(5)]
        [MinLength(15)]
        public string Country { get; set; } = null!;
    }
}
