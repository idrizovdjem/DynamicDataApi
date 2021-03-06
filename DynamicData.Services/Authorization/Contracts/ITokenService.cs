using System.Threading.Tasks;
using DynamicData.DTOs.Token;
using DynamicData.Data.Models;

namespace DynamicData.Services.Authorization.Contracts
{
    public interface ITokenService
    {
        Task<TokenResult> GenerateTokensAsync(ApplicationUser user);

        AccessToken GetAccessToken(string token, string refreshToken);
    }
}
