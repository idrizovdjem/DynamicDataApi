using DynamicData.Web.Services;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DynamicData.Web.Models.Token;

namespace DynamicData.Web.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TokenController : Controller
    {
        private readonly IUtilitiesService utilitiesService;
        private readonly ITokenService tokenService;
        private readonly IUsersService usersService;

        public TokenController(IUtilitiesService utilitiesService, ITokenService tokenService, IUsersService usersService)
        {
            this.utilitiesService = utilitiesService;
            this.tokenService = tokenService;
            this.usersService = usersService;
        }

        [HttpPost]
        public async Task<IActionResult> Refresh(RefreshTokenInputModel input)
        {
            var response = new ResponseModel();

            var headerToken = this.utilitiesService.GetAccessTokenHeader(HttpContext);
            if (headerToken == null)
            {
                response.AddErrorMessage("Missing access token header");
                response.StatusCode = 400;
                return Json(response);
            }

            var accessToken = this.tokenService.GetAccessToken(headerToken, input.RefreshToken);
            if (accessToken == null)
            {
                response.AddErrorMessage("Invalid refresh token");
                response.StatusCode = 400;
                return Json(response);
            }

            if (accessToken.RefreshExpirationDate < DateTime.UtcNow)
            {
                response.AddErrorMessage("Refresh token expired");
                response.StatusCode = 400;
                return Json(response);
            }
            var user = this.usersService.GetById(accessToken.UserId);

            var tokens = await this.tokenService.GenerateTokensAsync(user);

            response.Data = tokens;
            return Json(response);
        }
    }
}
