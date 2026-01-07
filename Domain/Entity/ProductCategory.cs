namespace Domain.Entity
{
    public class ProductCategory
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public long CategoryId { get; set; }

        public Product Product { get; set; } = null!;
        public Category Category { get; set; } = null!;
    }
}
