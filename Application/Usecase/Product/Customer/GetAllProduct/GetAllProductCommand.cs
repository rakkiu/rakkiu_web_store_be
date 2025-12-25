using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model.Product;
using MediatR;

namespace Application.Usecase.Product.Customer.GetAllProduct
{
    public record GetAllProductCommand : IRequest<List<GetAllProductDto>>;

   
}
