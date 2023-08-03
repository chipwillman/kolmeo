using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Kolmeo.Rest
{
    public class GetProductsModel 
    {
        [JsonProperty("skip")]
        public int Skip { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }
        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }
        [JsonProperty("products")]
        public List<GetProductModel> Products { get; set; } = new List<GetProductModel>();
    }
}
