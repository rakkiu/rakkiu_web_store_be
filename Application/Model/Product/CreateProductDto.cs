using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model.Product
{
    public class CreateProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public List<string>? ImageUrls { get; set; }
        public List<long>? CategoryIds { get; set; }
        public List<ProductSizeDto>? Sizes { get; set; }
    }
}
