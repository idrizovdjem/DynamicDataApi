using System.Collections.Generic;
using DynamicData.DTOs.Column;

namespace DynamicData.Services.Application.Contracts
{
    public interface IColumnsService
    {
        ColumnValidationResult ValidateColumnNames(List<CreateColumnInputModel> columns);

        void ProccessIdentifier(List<CreateColumnInputModel> columns, string tableName);

        string GenerateSqlQuery(List<CreateColumnInputModel> columns, string tableName);
    }
}
