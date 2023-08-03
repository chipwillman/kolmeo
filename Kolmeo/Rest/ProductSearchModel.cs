namespace Kolmeo.Rest
{
    public class ProductSearchModel
    {
        public int? Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
