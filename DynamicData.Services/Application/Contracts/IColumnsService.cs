using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using DynamicData.DTOs.Column;
using DynamicData.Common.Enums;

namespace DynamicData.Services.Application.Contracts
{
    public interface IColumnsService
    {
        ColumnValidationResult ValidateColumnNames(List<CreateColumnInputModel> columns);

        void ProccessIdentifier(List<CreateColumnInputModel> columns, string tableName);

        string GenerateSqlQuery(List<CreateColumnInputModel> columns, string tableName);

        Type ConvertColumnType(ColumnType type);

        Task<CreateColumnInputModel[]> GetTableColumnsAsync(string tableName, string userId);
    }
}
