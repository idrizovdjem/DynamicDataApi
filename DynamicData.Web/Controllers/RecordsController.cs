using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DynamicData.DTOs.Column;
using Microsoft.AspNetCore.Authorization;
using DynamicData.Services.Application.Contracts;
using DynamicData.DTOs.Record;

namespace DynamicData.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RecordsController : ControllerBase
    {
        private readonly ITablesService tablesService;
        private readonly IColumnsService columnsService;
        private readonly IRecordsService recordsService;

        public RecordsController(
            ITablesService tablesService,
            IColumnsService columnsService,
            IRecordsService recordsService)
        {
            this.tablesService = tablesService;
            this.columnsService = columnsService;
            this.recordsService = recordsService;
        }

        [HttpPost]
        [Route("{tableName}")]
        public async Task<ActionResult> OnPostAsync(string tableName, CreateRecordColumnsInputModel input)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(await this.tablesService.TableExistsAsync(tableName, userId) == false)
            {
                return BadRequest("Table with that name does not exist!");
            }

            var tableColumns = await this.columnsService.GetTableColumnsAsync(tableName, userId);
            if(tableColumns.Length == 0)
            {
                return BadRequest("Table has no columns");
            }

            // dont comapre by name, but instead compare by internal name
            var recordValidationResult = this.recordsService.ColumnRequirementsSatisfied(tableName, userId, tableColumns, input.Columns);
            if(recordValidationResult.IsSuccessfull == false)
            {
                return BadRequest(recordValidationResult.ErrorMessage);
            }

            return Ok();
        }
    }
}
