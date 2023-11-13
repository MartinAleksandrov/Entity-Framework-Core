using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CarDealer.DTOs.Import
{
    public class ImportSalesDto
    {
        [JsonProperty("carId")]
        public int carId { get; set; }

        [JsonProperty("customerId")]
        public int customerId { get; set; }

        [JsonProperty("discount")]
        public int discount { get; set; }
    }
}
