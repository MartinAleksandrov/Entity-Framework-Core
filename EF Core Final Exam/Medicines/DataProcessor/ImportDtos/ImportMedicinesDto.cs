using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType("Medicine")]
    public class ImportMedicinesDto
    {
        [Required]
        [XmlAttribute("category")]
        [Range(0, 4)]
        public int Category { get; set; }

        [Required]
        [XmlElement("Name")]
        [MinLength(3)]
        [MaxLength(150)]
        public string Name { get; set; } = null!;

        [Required]
        [XmlElement("Price")]
        [Range(0.01,1000)]
        public decimal Price { get; set; }

        [Required]
        [XmlElement("ProductionDate")]
        public string ProductionDate { get; set; } = null!;

        [Required]
        [XmlElement("ExpiryDate")]
        public string ExpiryDate { get; set; } = null!;


        [Required]
        [XmlElement("Producer")]
        [MinLength(3)]
        [MaxLength(100)]
        public string Producer { get; set; } = null!;

    }
}
