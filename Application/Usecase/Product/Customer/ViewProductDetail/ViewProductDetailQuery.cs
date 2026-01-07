using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Model.Product;
using MediatR;

namespace Application.Usecase.Product.Customer.ViewProductDetail
{
    public class ViewProductDetailQuery : IRequest<ViewProductDetailDto>
    {
        public Guid ProductId { get; init; }
    }
}
