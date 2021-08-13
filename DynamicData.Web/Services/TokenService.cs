using DynamicData.Web.Data;
using DynamicData.Web.Data.Models;
using DynamicData.Web.Models.Token;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DynamicData.Web.Services
{
    public class TokenService : ITokenService
    {
        private readonly TokenConfig tokenConfig;
        private readonly ApplicationDbContext dbContext;

        public TokenService(TokenConfig jwtTokenConfig, ApplicationDbContext dbContext)
        {
            this.tokenConfig = jwtTokenConfig;
            this.dbContext = dbContext;
        }

        public string BuildToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfig.Secret));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    issuer: tokenConfig.Issuer,
                    audience: tokenConfig.Audience,
                    notBefore: DateTime.Now,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(tokenConfig.AccessTokenExpiration),
                    signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string BuildRefreshToken()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<TokenResult> GenerateTokensAsync(ApplicationUser user)
        {
            if (user == null)
            {
                return null;
            }

            var claims = BuildClaims(user);
            var accessToken = new AccessToken()
            {
                Token = BuildToken(claims),
                RefreshToken = BuildRefreshToken(),
                User = user,
                RefreshExpirationDate = DateTime.UtcNow.AddMinutes(30)
            };

            await this.dbContext.AccessTokens.AddAsync(accessToken);
            await this.dbContext.SaveChangesAsync();

            return new TokenResult()
            {
                AccessToken = accessToken.Token,
                RefreshToken = accessToken.RefreshToken
            };
        }

        private static Claim[] BuildClaims(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            };

            return claims;
        }

        public AccessToken GetAccessToken(string token, string refreshToken)
        {
            return this.dbContext.AccessTokens
                .FirstOrDefault(at => at.Token == token && at.RefreshToken == refreshToken);
        }
    }
}
