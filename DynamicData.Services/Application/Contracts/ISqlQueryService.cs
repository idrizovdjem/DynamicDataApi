using System.Threading.Tasks;

namespace DynamicData.Services.Application.Contracts
{
    public interface ISqlQueryService
    {
        Task CreateTableAsync(string sqlQuery);
    }
}
