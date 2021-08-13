using System.Threading.Tasks;
using DynamicData.DTOs.Table;

namespace DynamicData.Services.Application.Contracts
{
    public interface ITablesService
    {
        Task<TableRecordViewModel[]> GetAllTablesAsync(string userId);
    }
}
