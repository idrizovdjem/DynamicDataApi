using DynamicData.Web.Data.Models;
using DynamicData.Web.Models.Token;
using System.Threading.Tasks;

namespace DynamicData.Web.Services
{
    public interface ITokenService
    {
        Task<TokenResult> GenerateTokensAsync(ApplicationUser user);

        AccessToken GetAccessToken(string token, string refreshToken);
    }
}
