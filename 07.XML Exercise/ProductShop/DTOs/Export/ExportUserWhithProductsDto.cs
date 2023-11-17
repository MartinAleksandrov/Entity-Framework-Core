using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    [XmlType("User")]
    public class ExportUserWhithProductsDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; } = null!;


        [XmlElement("lastName")]
        public string LastName { get; set; } = null!;


        [XmlElement("age")]
        public int? Age { get; set; }


        //This prop needs additional mapping
        [XmlElement("SoldProducts")]
        public ExpotProductNestedDto SoldProducts { get; set; }




    }
}
