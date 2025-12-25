using Application.Model.Auth.Login;
using MediatR;
using Presentation.Common;
using Microsoft.AspNetCore.Mvc;
using Application.Usecase.Auth.Login;
using Application.Usecase.Auth.Logout;
using Application.Usecase.Auth.AccountRegister;
namespace Presentation.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), 200)]
        public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginCommand command)
        {
            var res = await _mediator.Send(command);
            return Ok(new ApiResponse<LoginResponseDto>
            {
                StatusCode = 200,
                Message = "Login successful",
                Data = res,
                ResponsedAt = DateTime.UtcNow
            });
        }

        [HttpPost("logout")]
        [ProducesResponseType(typeof (ApiResponse<>), 200)]
        [ProducesResponseType(typeof(ApiResponse<>), 400)]

        public async Task<ActionResult<ApiResponse<object?>>> Logout([FromBody] LogoutCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return Ok(new ApiResponse<object?>
            {
                StatusCode = 200,
                Message = "Logout successful",
                Data = null,
                ResponsedAt = DateTime.UtcNow
            });
        }

        [HttpPost("account-register")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]

        public async Task<ActionResult<ApiResponse<object>>> AccountRegister([FromBody] AccountRegisterCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _mediator.Send(command, cancellationToken);
                return Ok(new ApiResponse<object>
                {
                    StatusCode = 200,
                    Message = "Account registration successful",
                    Data = res,
                    ResponsedAt = DateTime.UtcNow
                });
            }
            catch
            {
                return Ok(new ApiResponse<object>
                {
                    StatusCode = 400,
                    Message = "This email already created!",
                    Data = null,
                    ResponsedAt = DateTime.UtcNow
                });
            }
        }
    }
}
