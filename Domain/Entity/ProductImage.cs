namespace Domain.Entity
{
    public class ProductImage
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string? AltText { get; set; }

        public Product Product { get; set; } = null!;
    }
}
