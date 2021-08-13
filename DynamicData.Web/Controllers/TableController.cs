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
        private readonly IColumnsService columnsService;
        private readonly ISqlQueryService sqlQueryService;

        public TableController(
            ITablesService tablesService,
            IColumnsService columnsService,
            ISqlQueryService sqlQueryService)
        {
            this.tablesService = tablesService;
            this.columnsService = columnsService;
            this.sqlQueryService = sqlQueryService;
        }

        [HttpGet]
        public async Task<TableRecordViewModel[]> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await this.tablesService.GetAllTablesAsync(userId);
        }

        [HttpPost]
        public async Task<ActionResult> OnPostAsync(CreateTableInputModel input)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(this.tablesService.NameContainsInvalidCharacters(input.Name))
            {
                return BadRequest("Table name must contain only letters!");
            }

            var isTableNameUnique = await this.tablesService.IsNameUniqueAsync(input.Name, userId);
            if(isTableNameUnique == false)
            {
                return BadRequest("Table name must be unique!");
            }

            var columnValidationResult = this.columnsService.ValidateColumnNames(input.Columns);
            if(columnValidationResult.IsSuccessfull == false)
            {
                return BadRequest(columnValidationResult.ErrorMessage);
            }

            this.columnsService.ProccessIdentifier(input.Columns, input.Name);

            var tableName = input.Name + userId.Replace("-", string.Empty);
            var createTableSql = this.tablesService.GenerateSqlQuery(input, userId);
            await this.sqlQueryService.CreateTableAsync(createTableSql);
            await this.tablesService.CreateRecordAsync(tableName, userId);

            return Ok();
        }
    }
}
