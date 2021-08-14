using System.Threading.Tasks;
using DynamicData.DTOs.Table;

namespace DynamicData.Services.Application.Contracts
{
    public interface ITablesService
    {
        Task<TableRecordViewModel[]> GetAllTablesAsync(string userId);

        string GenerateSqlQuery(CreateTableInputModel input, string userId);

        Task<TableRecordViewModel> CreateRecordAsync(CreateTableInputModel input, string userId);

        bool NameContainsInvalidCharacters(string tableName);

        Task<TableRecordViewModel> GetByNameAsync(string tableName, string userId);

        Task<bool> TableExistsAsync(string tableName, string userId);
    }
}
