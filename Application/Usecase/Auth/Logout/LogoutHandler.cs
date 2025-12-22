using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Domain.Interface;
namespace Application.Usecase.Auth.Logout
{
    public class LogoutHandler : IRequestHandler<LogoutCommand, bool>
    {

        private readonly IJwtTokenRepository _jwtTokenRepo;

        public LogoutHandler(IJwtTokenRepository jwtTokenRepo)
        {
            _jwtTokenRepo = jwtTokenRepo;
        }
        public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.refreshToken))
                throw new ArgumentException("Refresh token cannot be null or empty.");

            var refreshToken = await _jwtTokenRepo.GetByTokenAsync(request.refreshToken);
            if(refreshToken == null || refreshToken.IsRevoked || refreshToken.ExpiresAt <= DateTime.UtcNow)
                throw new UnauthorizedAccessException("Invalid refresh token.");

            refreshToken.IsRevoked = true;

            _jwtTokenRepo.UpdateAsync(refreshToken, cancellationToken);

            var accessTokens = await _jwtTokenRepo.GetAccessTokenByUserIdAsync(refreshToken.UserId);
            if(accessTokens != null)
            {
              accessTokens.IsRevoked = true;
              await _jwtTokenRepo.UpdateAsync(accessTokens, cancellationToken);
            }

            await _jwtTokenRepo.SaveChangeAsync();
            return true;
        }

        
    }
}
