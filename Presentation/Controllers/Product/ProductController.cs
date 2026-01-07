using Application.Model.Auth.Login;
using Application.Model.Product;
using Application.Usecase.Auth.Login;
using Application.Usecase.Product.Customer.GetAllProduct;
using Application.Usecase.Product.Customer.ViewProductDetail;
using Application.Usecase.Product.Manage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;
using Presentation.Common;

namespace Presentation.Controllers.Product
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResponse<GetAllProductDto>), 200)]

        public async Task<ActionResult<ApiResponse<GetAllProductDto>>> GetAllProducts(CancellationToken cancellationToken)
        {
            var res = await _mediator.Send(new GetAllProductQuery());
            return Ok(new ApiResponse<IEnumerable<GetAllProductDto>>
            {
                StatusCode = 200,
                Message = "Get all products successful",
                Data = res,
                ResponsedAt = DateTime.UtcNow
            });
        }
 
        [Authorize(Roles = "Seller")]
        [HttpPost("add-new")]
        [ProducesResponseType(typeof(ApiResponse<CreateProductDto>), 200)]

        public async Task<ActionResult<ApiResponse<CreateProductDto>>> AddNewProduct([FromBody] AddNewProductCommand addNewProductCommand)
        {
            var res = await _mediator.Send(addNewProductCommand);
            try
            {
                return Ok(new ApiResponse<CreateProductDto>
                {
                    StatusCode = 200,
                    Message = "Add new product successful",
                    Data = res,
                    ResponsedAt = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.UtcNow
                });


            }

            
        }

        [HttpGet("view-detail")]
        public async Task<ActionResult<ApiResponse<ViewProductDetailDto>>> ViewDetail([FromQuery] Guid productId)
        {
            var viewProductDetailCommand = new ViewProductDetailQuery { ProductId = productId };
            var res = await _mediator.Send(viewProductDetailCommand);
            return Ok(new ApiResponse<ViewProductDetailDto>
            {
                StatusCode = 200,
                Message = "View product detail successful",
                Data = res,
                ResponsedAt = DateTime.UtcNow
            });
        }
    }
}
