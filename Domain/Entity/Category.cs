namespace Domain.Entity
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
    }
}
