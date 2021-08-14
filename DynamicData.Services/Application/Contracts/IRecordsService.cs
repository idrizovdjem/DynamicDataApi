using DynamicData.DTOs.Column;
using DynamicData.DTOs.Record;

namespace DynamicData.Services.Application.Contracts
{
    public interface IRecordsService
    {
        RecordValidationResult ColumnRequirementsSatisfied(
            string tableName,
            string userId,
            CreateColumnInputModel[] tableColumns, 
            CreateColumnValuesInputModel[] valueColumns);
    }
}
