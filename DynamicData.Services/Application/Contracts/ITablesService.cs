using System.Threading.Tasks;
using DynamicData.DTOs.Table;

namespace DynamicData.Services.Application.Contracts
{
    public interface ITablesService
    {
        Task<TableRecordViewModel[]> GetAllTablesAsync(string userId);

        Task<bool> IsNameUniqueAsync(string tableName, string userId);

        string GenerateSqlQuery(CreateTableInputModel input, string userId);

        Task CreateRecordAsync(string tableName, string userId);

        bool NameContainsInvalidCharacters(string tableName);
    }
}
