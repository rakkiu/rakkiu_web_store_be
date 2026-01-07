using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Domain.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> AddProductAsync(Product product);
        Task<bool> GetByNameAsync(string name);
        Task<bool> CategoriesExistAsync(IEnumerable<long> categoryIds);

        Task<Product?> GetProductByIdAsync(Guid productId);
    }
}
