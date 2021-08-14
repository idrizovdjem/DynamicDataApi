using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DynamicData.Data;
using DynamicData.DTOs.Column;
using DynamicData.DTOs.Record;
using DynamicData.Common.Enums;
using DynamicData.Services.Application.Contracts;

namespace DynamicData.Services.Application
{
    public class RecordsService : IRecordsService
    {
        private readonly IColumnsService columnsService;
        private readonly ApplicationDbContext context;

        public RecordsService(
            IColumnsService columnsService,
            ApplicationDbContext context)
        {
            this.columnsService = columnsService;
            this.context = context;
        }

        public RecordValidationResult ColumnRequirementsSatisfied(
            string tableName,
            string userId,
            CreateColumnInputModel[] tableColumns, 
            CreateColumnValuesInputModel[] valueColumns)
        {
            var validationResult = new RecordValidationResult();

            var requiredColumnsSatisifed = CheckRequiredColumns(tableColumns, valueColumns);
            if(requiredColumnsSatisifed == false)
            {
                validationResult.IsSuccessfull = false;
                validationResult.ErrorMessage = "Not all required columns are satisfied";
                return validationResult;
            }

            var uniqueColumnsSatisfied = this.CheckUniqueColumns(tableName, userId, tableColumns, valueColumns);
            if(uniqueColumnsSatisfied == false)
            {
                validationResult.IsSuccessfull = false;
                validationResult.ErrorMessage = "Not all unique columns are satisfied";
                return validationResult;
            }

            return validationResult;
        }

        private static bool CheckRequiredColumns(
            CreateColumnInputModel[] tableColumns,
            CreateColumnValuesInputModel[] valueColumns)
        {
            var requiredColumns = tableColumns
                .Where(tc => tc.IsRequired == true)
                .ToArray();

            if(requiredColumns.Length > valueColumns.Length)
            {
                return false;
            }

            foreach(var requiredColumn in requiredColumns)
            {
                var valueColumn = valueColumns
                    .FirstOrDefault(vc => vc.Name == requiredColumn.Name);

                if(valueColumn == null)
                {
                    return false;
                }

                var isValidType = true;

                try
                {
                    if(requiredColumn.Type == ColumnType.Text)
                    {
                        Convert.ChangeType(valueColumn.Value, typeof(string));
                    } 
                    else if(requiredColumn.Type == ColumnType.Integer)
                    {
                        Convert.ChangeType(valueColumn.Value, typeof(int));
                    } 
                    else if(requiredColumn.Type == ColumnType.Decimal)
                    {
                        Convert.ChangeType(valueColumn.Value, typeof(decimal));
                    }
                    else if(requiredColumn.Type == ColumnType.Boolean)
                    {
                        Convert.ChangeType(valueColumn.Value, typeof(bool));
                    }
                }
                catch(Exception ex)
                {
                    isValidType = false;
                }

                if(isValidType == false)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckUniqueColumns(
            string tableName,
            string userId,
            CreateColumnInputModel[] tableColumns,
            CreateColumnValuesInputModel[] valueColumns)
        {
            var uniqueColumns = tableColumns
                .Where(tc => tc.IsUnique == true)
                .ToArray();

            foreach (var uniqueColumn in uniqueColumns)
            {
                var valueColumn = valueColumns
                    .FirstOrDefault(vc => vc.Name == uniqueColumn.Name);

                if(valueColumn == null)
                {
                    continue;
                }

                var tableNameString = this.context.TableRecords
                    .Where(tr => tr.Name == tableName && tr.CreatorId == userId && tr.IsDeleted == false)
                    .Select(tr => tr.InternalName)
                    .FirstOrDefault();

                var sqlString = $"SELECT COUNT({uniqueColumn.Name}) FROM {tableNameString} WHERE {uniqueColumn.Name} = '@param'";

                var matchesCount = this.context.Database.ExecuteSqlRaw(sqlString, valueColumn.Value);
                if(matchesCount > 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
