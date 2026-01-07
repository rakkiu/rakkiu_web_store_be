using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model.Product
{
    public class ViewProductDetailDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }

        public ICollection<ProductImageDto> ProductImages { get; set; } = new List<ProductImageDto>();

        public ICollection<ProductSizeDto> Size { get; set; } = new List<ProductSizeDto>();
    }
}
