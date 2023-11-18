using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Export
{
    [XmlType("sale")]
    public class ExportSalesDto
    {
        [XmlElement("car")]
        public ExportCarsDto Car { get; set; }


        [XmlElement("discount")]
        public decimal Discount { get; set; }


        [XmlElement("customer-name")]
        public string CustomerName { get; set; } = null!;


        [XmlElement("price")]
        public decimal Price { get; set; }


        [XmlElement("price-with-discount")]
        public decimal PriceWithDiscount
        {
            get
            {
                if (IsYoungDriver)
                {
                    return Price;
                }
                return Price - (Price * Discount / 100);
            }
            set { }
        }

        [XmlIgnore]
        public bool IsYoungDriver { get; set; }

    }
}
