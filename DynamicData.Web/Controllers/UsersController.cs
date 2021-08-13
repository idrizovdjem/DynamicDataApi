using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DynamicData.DTOs.User;
using DynamicData.DTOs.Token;
using DynamicData.Services.Application.Contracts;

namespace DynamicData.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService usersService;
        private readonly IUtilitiesService utilitiesService;
        private readonly ITokenService tokenAuthService;

        public UsersController(IUsersService usersService, ITokenService tokenAuthService, IUtilitiesService utilitiesService)
        {
            this.usersService = usersService;
            this.tokenAuthService = tokenAuthService;
            this.utilitiesService = utilitiesService;
        }

        [HttpPost]
        public async Task<ResponseModel> Register(UserRegisterInputModel input)
        {
            var response = new ResponseModel();
            if (!ModelState.IsValid)
            {
                var errorMessages = this.utilitiesService.GetModelStateErorrs(ModelState);
                foreach (var message in errorMessages)
                {
                    response.AddErrorMessage(message);
                }

                response.StatusCode = 400;
                return response;
            }

            var isEmailAvailable = this.usersService.IsEmailAvailable(input.Email);

            if (isEmailAvailable == false)
            {
                response.AddErrorMessage("This email is already taken");
                response.StatusCode = 400;
                return response;
            }

            var isUsernameAvailable = this.usersService.IsUsernameAvailable(input.Username);

            if (isUsernameAvailable == false)
            {
                response.AddErrorMessage("This username is already taken");
                response.StatusCode = 400;
                return response;
            }

            var user = await this.usersService.RegisterAsync(input);
            var tokens = await this.tokenAuthService.GenerateTokensAsync(user);

            response.Data = tokens;
            return response;
        }


        [HttpPost]
        public async Task<ResponseModel> Login(UserLoginInputModel input)
        {
            var response = new ResponseModel();
            if (!ModelState.IsValid)
            {
                var errorMessage = this.utilitiesService.GetModelStateErorrs(ModelState);
                foreach (var message in errorMessage)
                {
                    response.AddErrorMessage(message);
                }

                response.StatusCode = 400;
                return response;
            }

            var user = this.usersService.Login(input);
            if (user == null)
            {
                response.AddErrorMessage("Invalid login information");
                response.StatusCode = 401;
                return response;
            }

            var tokens = await this.tokenAuthService.GenerateTokensAsync(user);
            response.Data = tokens;
            return response;
        }
    }
}
