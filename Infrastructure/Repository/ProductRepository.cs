using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Interface;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            var prod = await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            
            // Reload the product with all related data
            return await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductCategories)
                .Include(p => p.ProductSizes)
                .FirstAsync(p => p.Id == prod.Entity.Id);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductCategories)
                .Include(p => p.ProductSizes)
                .ToListAsync();
        }

        public async Task<bool> GetByNameAsync(string name)
        {
            var exists = await _context.Products.FirstOrDefaultAsync(p => p.Name == name);
            return exists != null;
        }

        public async Task<bool> CategoriesExistAsync(IEnumerable<long> categoryIds)
        {
            if (categoryIds == null || !categoryIds.Any())
                return true;

            var existingCategoryIds = await _context.Categories
                .Where(c => categoryIds.Contains(c.Id))
                .Select(c => c.Id)
                .ToListAsync();

            return existingCategoryIds.Count == categoryIds.Distinct().Count();
        }

        public async Task<Product?> GetProductByIdAsync(Guid productId)
        {
            var existingProduct = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductCategories)
                .Include(p => p.ProductSizes)
                .FirstOrDefaultAsync(p => p.Id == productId);

            return existingProduct;
        }
    }
}
