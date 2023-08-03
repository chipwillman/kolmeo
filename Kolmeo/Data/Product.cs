using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kolmeo.Data
{
    public class Product
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; } = "";

        [Column(TypeName = "nvarchar(max)")]
        public string? Description { get; set; }
        [Column(TypeName = "decimal(16, 4)")]
        public decimal Price { get; set; } = 0m;
        public bool IsDeleted { get; set; } = false;
    }
}
