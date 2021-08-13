using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DynamicData.DTOs.Table;
using DynamicData.Services.Application.Contracts;

namespace DynamicData.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TableController : ControllerBase
    {
        private readonly ITablesService tablesService;

        public TableController(ITablesService tablesService)
        {
            this.tablesService = tablesService;
        }

        [HttpGet]
        public async Task<TableRecordViewModel[]> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await this.tablesService.GetAllTablesAsync(userId);
        }
    }
}
