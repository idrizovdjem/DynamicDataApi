using System.Linq;
using System.Threading.Tasks;
using DynamicData.Data;
using DynamicData.DTOs.User;
using DynamicData.Data.Models;
using DynamicData.Services.Common.Contracts;
using DynamicData.Services.Authorization.Contracts;

namespace DynamicData.Services.Authorization
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IUtilitiesService utilitiesService;

        public UsersService(ApplicationDbContext dbContext, IUtilitiesService utilitiesService)
        {
            this.dbContext = dbContext;
            this.utilitiesService = utilitiesService;
        }

        public async Task<ApplicationUser> RegisterAsync(UserRegisterInputModel input)
        {
            var hashedPassword = this.utilitiesService.HashPassword(input.Password);

            var user = new ApplicationUser()
            {
                Email = input.Email,
                UserName = input.Username,
                PasswordHash = hashedPassword
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.SaveChangesAsync();

            return user;
        }

        public ApplicationUser Login(UserLoginInputModel input)
        {
            var hashedPassword = this.utilitiesService.HashPassword(input.Password);

            return this.dbContext.Users
                .FirstOrDefault(u => u.Email == input.Email && u.PasswordHash == hashedPassword);
        }

        public bool IsEmailAvailable(string email)
        {
            return this.dbContext.Users
                .All(u => u.Email != email);
        }

        public bool IsUsernameAvailable(string username)
        {
            return this.dbContext.Users
                .All(u => u.UserName != username);
        }

        public ApplicationUser GetById(string userId)
        {
            return this.dbContext.Users
                .FirstOrDefault(u => u.Id == userId);
        }
    }
}
