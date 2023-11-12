using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductShop.DTOs.Import
{
    public class ImportCategoriesDto
    {
        [JsonProperty("name")]
        public string? Name { get; set;}
    }
}
