using System;

namespace Application.Model.Product
{
    public class ProductImageDto
    {
        public string ImageUrl { get; set; } = null!;
        public string? AltText { get; set; }
    }
}
