using Application.Model.Product;
using Domain.Interface;
using MediatR;

namespace Application.Usecase.Product.Manage
{
    public class AddNewProductHandler : IRequestHandler<AddNewProductCommand, CreateProductDto>
    {
        private readonly IProductRepository _productRepository;

        public AddNewProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<CreateProductDto> Handle(AddNewProductCommand request, CancellationToken cancellationToken)
        {
            var exists = await _productRepository.GetByNameAsync(request.Name);
            if (exists)
            {
                throw new Exception("Product with the same name already exists.");
            }

            var productId = Guid.NewGuid();
            
            var newProduct = new Domain.Entity.Product
            {
                Id = productId,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ProductImages = request.Images.Select(img => new Domain.Entity.ProductImage
                {
                    ImageUrl = img.ImageUrl,
                    AltText = img.AltText
                }).ToList(),
                ProductCategories = request.CategoryIds.Select(catId => new Domain.Entity.ProductCategory
                {
                    Id = Guid.NewGuid(),
                    ProductId = productId,
                    CategoryId = catId
                }).ToList(),
                ProductSizes = request.Sizes.Select((size, index) => new Domain.Entity.ProductSize
                {
                    Id = Guid.NewGuid(),
                    ProductId = productId,
                    Size = size.Size,
                    Stock = size.Stock,
                    DisplayOrder = index
                }).ToList()
            };

            var addedProduct = await _productRepository.AddProductAsync(newProduct);

            return new CreateProductDto
            {
                Id = addedProduct.Id,
                Name = addedProduct.Name,
                Description = addedProduct.Description,
                Price = addedProduct.Price,
                ImageUrls = addedProduct.ProductImages.Select(img => img.ImageUrl).ToList(),
                CategoryIds = addedProduct.ProductCategories.Select(pc => pc.CategoryId).ToList(),
                Sizes = addedProduct.ProductSizes
                    .OrderBy(ps => ps.DisplayOrder)
                    .Select(ps => new ProductSizeDto
                    {
                        Size = ps.Size,
                        Stock = ps.Stock
                    }).ToList()
            };
        }
    }
}
