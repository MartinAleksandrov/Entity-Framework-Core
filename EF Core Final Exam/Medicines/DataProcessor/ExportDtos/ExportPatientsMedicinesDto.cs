using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos
{
    [XmlType("Patient")]
    public class ExportPatientsMedicinesDto
    {
        [Required]
        [XmlAttribute("Gender")]
        public string Gender { get; set; } = null!;

        [Required]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [XmlElement("AgeGroup")]
        public string AgeGroup { get; set; } = null!;


        [XmlArray("Medicines")]
        public ExportMedicinesDto[] Medicines { get; set; } = null!;
    }
}
