using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model.Product;
using MediatR;

namespace Application.Usecase.Product.Manage
{
    public record AddNewProductCommand : IRequest<CreateProductDto>
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
        public decimal Price { get; init; }
        public List<long> CategoryIds { get; init; } = new();
        public List<ProductImageDto> Images { get; init; } = new();
        public List<ProductSizeDto> Sizes { get; init; } = new();

    }
}
