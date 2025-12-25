

using Application.Model.Auth.AccountRegister;
using MediatR;

namespace Application.Usecase.Auth.AccountRegister
{
    public record AccountRegisterCommand(
        string Email,
        string Password,
        string? FullName,
        string? PhoneNumber,
        DateTime DateOfBirth
    ) : IRequest<AccountRegisterResponseDto>;

}
