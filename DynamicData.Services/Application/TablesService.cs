using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; 
using DynamicData.Data;
using DynamicData.DTOs.Table;
using DynamicData.Services.Application.Contracts;

namespace DynamicData.Services.Application
{
    public class TablesService : ITablesService
    {
        private readonly ApplicationDbContext context;

        public TablesService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<TableRecordViewModel[]> GetAllTablesAsync(string userId)
        {
            return await this.context.TableRecords
                .Where(tr => tr.CreatorId == userId && tr.IsDeleted == false)
                .Select(tr => new TableRecordViewModel()
                {
                    Name = tr.Name,
                    CreatedOn = tr.CreatedOn.ToShortDateString(),
                    ModifiedOn = tr.ModifiedOn.ToShortDateString()
                })
                .ToArrayAsync();
        }
    }
}
