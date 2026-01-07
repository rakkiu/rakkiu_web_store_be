using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model.Product;
using Domain.Interface;
using MediatR;

namespace Application.Usecase.Product.Customer.ViewProductDetail
{
    public class ViewProductDetailHandler : IRequestHandler<ViewProductDetailQuery, ViewProductDetailDto>
    {
        private readonly IProductRepository _productRepository;
        public ViewProductDetailHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<ViewProductDetailDto> Handle(ViewProductDetailQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductByIdAsync(request.ProductId);

            if (product == null)
            {
                throw new Exception("this product does not exist");

            }
            return new ViewProductDetailDto
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Size = product.ProductSizes
                    .OrderBy(size => size.DisplayOrder)
                    .Select(size => new ProductSizeDto
                    {
                        Size = size.Size,
                        Stock = size.Stock
                    }).ToList(),
                ProductImages = product.ProductImages.Select(img => new ProductImageDto
                {
                    ImageUrl = img.ImageUrl,
                    AltText = img.AltText
                }).ToList(),
            };

        }
    }
}
