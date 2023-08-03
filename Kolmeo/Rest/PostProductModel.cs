using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Kolmeo.Rest
{
    public class PostProductModel : IValidatableObject
    {
        [Required]
        [JsonProperty("name")]
        public string Name { get; set; } = "";
        [JsonProperty("description")]
        public string? Description { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(Name, new ValidationContext(this, null, null) { MemberName = "Name" }, results);
            if (this.Price < 0m)
            {
                results.Add(new ValidationResult("the price must be positive"));
            }
            return results;
        }
    }
}
