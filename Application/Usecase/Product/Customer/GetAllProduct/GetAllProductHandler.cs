using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model.Product;
using Domain.Interface;
using MediatR;

namespace Application.Usecase.Product.Customer.GetAllProduct
{
    public class GetAllProductHandler : IRequestHandler<GetAllProductQuery, IEnumerable<GetAllProductDto>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
                                                                        
        public async Task<IEnumerable<GetAllProductDto>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
           var products = await _productRepository.GetAllProductsAsync();
            var productDtos = products.Select(p => new GetAllProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ProductImages = p.ProductImages.Select(img => new ProductImageDto
                {
                    ImageUrl = img.ImageUrl,
                    AltText = img.AltText
                }).ToList()
            });
            return productDtos;
        }
    }
}
