namespace Domain.Entity
{
    public class ProductSize
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Size { get; set; } = null!; // e.g., "S", "M", "L", "XL", "38", "39", "40"
        public int Stock { get; set; } // Stock for this specific size
        public int DisplayOrder { get; set; } // Order for displaying sizes
        
        public Product Product { get; set; } = null!;
    }
}
