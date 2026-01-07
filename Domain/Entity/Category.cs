namespace Domain.Entity
{
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
    }
}
