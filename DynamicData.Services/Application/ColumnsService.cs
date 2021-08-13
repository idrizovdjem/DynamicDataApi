using System.Linq;
using System.Text;
using System.Collections.Generic;
using DynamicData.DTOs.Column;
using DynamicData.Common.Enums;
using DynamicData.Services.Application.Contracts;

namespace DynamicData.Services.Application
{
    public class ColumnsService : IColumnsService
    {
        public void ProccessIdentifier(List<CreateColumnInputModel> columns, string tableName)
        {
            var identifierColumn = columns
                .FirstOrDefault(c => c.Name.ToLower() == "id" || c.Name.ToLower() == tableName.ToLower() + "id");

            if(identifierColumn == null)
            {
                identifierColumn = AddIdentifierColumn(columns);
            }

            if(identifierColumn.Type != ColumnType.Text)
            {
                identifierColumn.Type = ColumnType.Text;
            }
        }

        public ColumnValidationResult ValidateColumnNames(List<CreateColumnInputModel> columns)
        {
            var validationResult = new ColumnValidationResult();

            var columnNames = new List<string>();
            foreach (var column in columns)
            {
                if (columnNames.Contains(column.Name))
                {
                    validationResult.IsSuccessfull = false;
                    validationResult.ErrorMessage = "There are columns with duplicate names!";
                    return validationResult;
                }

                if(NameContainsInvalidCharacters(column.Name))
                {
                    validationResult.IsSuccessfull = false;
                    validationResult.ErrorMessage = "Column name can contain only letters!";
                    return validationResult;
                }

                columnNames.Add(column.Name);
            }

            return validationResult;
        }

        public string GenerateSqlQuery(List<CreateColumnInputModel> columns, string tableName)
        {
            var identifierColumn = columns
                .FirstOrDefault(c => c.Name.ToLower() == "id" || c.Name.ToLower() == tableName.ToLower() + "id");

            var queryBuilder = new StringBuilder();
            queryBuilder.Append($"{identifierColumn.Name} VARCHAR(36) NOT NULL PRIMARY KEY,");

            foreach (var column in columns)
            {
                if (column == identifierColumn)
                {
                    continue;
                }

                var columnType = ConvertColumnType(column.Type);
                var canBeNull = column.IsRequired ? "NOT NULL" : string.Empty;
                var canBeUnique = column.IsUnique ? "UNIQUE" : string.Empty;

                queryBuilder.Append($"{column.Name} {columnType} {canBeNull} {canBeUnique},");
            }

            return queryBuilder.ToString();
        }

        private static CreateColumnInputModel AddIdentifierColumn(List<CreateColumnInputModel> columns)
        {
            var identifierColumn = new CreateColumnInputModel()
            {
                Name = "Id",
                Type = ColumnType.Text,
                IsRequired = true,
                IsUnique = true
            };

            columns.Insert(0, identifierColumn);
            return identifierColumn;
        }

        private static string ConvertColumnType(ColumnType type)
        {
            return type switch
            {
               ColumnType.Text => "NVARCHAR",
               ColumnType.Integer => "INT",
               ColumnType.Boolean => "BIT",
               ColumnType.Decimal => "DECIMAL",
               _ => "NVARCHAR"
            };
        }

        private static bool NameContainsInvalidCharacters(string columnName)
        {
            return columnName.Any(letter => char.IsLetter(letter) == false);
        }
    }
}
