using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos
{
    [XmlType("Medicine")]
    public class ExportMedicinesDto
    {
        [Required]
        [XmlAttribute("Category")]
        public string Category { get; set; } = null!;

        [Required]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;


        [Required]
        [XmlElement("Price")]
        public string Price { get; set; }

        [Required]
        [XmlElement("Producer")]
        public string Producer { get; set; } = null!;

        [Required]
        [XmlElement("BestBefore")]
        public string BestBefore { get; set; } = null!;
    }
}
