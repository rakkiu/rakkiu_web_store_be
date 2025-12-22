using Application.Interfaces;
using Domain.Interface;
using MediatR;
using Domain.Entity;
using Domain.Interfaces;
using Application.Model.Auth.Login;
namespace Application.Usecase.Auth.Login
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="IRequestHandler&lt;LoginCommand, LoginResultDto&gt;" />
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {


        /// <summary>

        /// The repo
        /// </summary>
        private readonly IUserRepository _repo;
        /// <summary>
        /// The JWT
        /// </summary>
        private readonly IJwtService _jwt;
        /// <summary>
        /// The JWT Token Repository
        /// </summary>
        private readonly IJwtTokenRepository _jwtTokenRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginHandler"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        /// <param name="jwt">The JWT.</param>
        /// <param name="jwtTokenRepo">The JWT token repository.</param>
        public LoginHandler(IUserRepository repo, IJwtService jwt, IJwtTokenRepository jwtTokenRepo)
        {
            _repo = repo;
            _jwt = jwt;
            _jwtTokenRepo = jwtTokenRepo;
        }

        /// <summary>
        /// Handles a request
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// Response from the request
        /// </returns>
        /// <exception cref="UnauthorizedAccessException">Invalid email or password.</exception>
        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Validate user credentials
            var user = await _repo.GetByEmailAsync(request.Email, cancellationToken);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password.");


            // Generate tokens
            var accessToken = _jwt.GenerateAccessToken(user);
            var refreshToken = _jwt.GenerateRefreshToken();

            // Get expiration times from settings
            var accessTokenExpirationMinutes = _jwt.GetAccessTokenExpirationMinutes();
            var refreshTokenExpirationDays = _jwt.GetRefreshTokenExpirationDays();

            // Save AccessToken to database
            var accessTokenEntity = new JwtToken
            {
                Token = accessToken,
                TokenType = "AccessToken",
                ExpiresAt = DateTime.UtcNow.AddMinutes(accessTokenExpirationMinutes), // Đảm bảo dùng UtcNow
                IsRevoked = false,
                UserId = user.Id
            };

            // Save RefreshToken to database
            var refreshTokenEntity = new JwtToken
            {
                Token = refreshToken,
                TokenType = "RefreshToken",
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpirationDays), // Đảm bảo dùng UtcNow
                IsRevoked = false,
                UserId = user.Id
            };

            // Save both tokens to database
            await _jwtTokenRepo.SaveTokenAsync(accessTokenEntity, cancellationToken);
            await _jwtTokenRepo.SaveTokenAsync(refreshTokenEntity, cancellationToken);
            _repo.Update(user); await _repo.SaveChangesAsync();
            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }
    }
}
