using Domain.Entity;
using Application.Model.Auth.AccountRegister;
using Domain.Interfaces;
using MediatR;

namespace Application.Usecase.Auth.AccountRegister
{
    public class AccountRegisterHandler : IRequestHandler<AccountRegisterCommand, AccountRegisterResponseDto>
    {
        private readonly IUserRepository _userRepository;

        public AccountRegisterHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<AccountRegisterResponseDto> Handle(AccountRegisterCommand request, CancellationToken cancellationToken)
        {
            var existEmail = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

            if(existEmail != null)
            {
                throw new Exception("Email already exists");
            }

            else
            {
                var user = new User
                {
                    Email = request.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    FullName = request.FullName,
                    PhoneNumber = request.PhoneNumber,
                    DateOfBirth = request.DateOfBirth,
                 
                };
                await _userRepository.AddAsync(user, cancellationToken);
                await _userRepository.SaveChangesAsync(cancellationToken);
                var response = new AccountRegisterResponseDto
                {
                    Email = request.Email,
                    FullName = request.FullName,
                    PhoneNumber = request.PhoneNumber,
                    DateOfBirth = request.DateOfBirth,
                };
                return response;
            }

        }
    }
}
