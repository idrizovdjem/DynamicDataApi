using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore; 
using DynamicData.Data;
using DynamicData.DTOs.Table;
using DynamicData.Data.Models;
using DynamicData.DTOs.Column;
using DynamicData.Services.Application.Contracts;

namespace DynamicData.Services.Application
{
    public class TablesService : ITablesService
    {
        private readonly IColumnsService columnsService;
        private readonly ApplicationDbContext context;

        public TablesService(
            ApplicationDbContext context,
            IColumnsService columnsService)
        {
            this.context = context;
            this.columnsService = columnsService;
        }

        public async Task<TableRecordViewModel> CreateRecordAsync(CreateTableInputModel input, string userId)
        {
            var tableRecord = new TableRecord()
            {
                InternalName = input.Name + userId,
                Name = input.Name,
                CreatorId = userId,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                Columns = JsonSerializer.Serialize(input.Columns)
            };

            await this.context.TableRecords.AddAsync(tableRecord);
            await this.context.SaveChangesAsync();

            return await this.GetByNameAsync(input.Name, userId);
        }

        public string GenerateSqlQuery(CreateTableInputModel input, string userId)
        {
            var tableName = input.Name + userId.Replace("-", string.Empty);
            var columnsSqlQuery = this.columnsService.GenerateSqlQuery(input.Columns, input.Name);
            return $"CREATE TABLE {tableName}({columnsSqlQuery})";
        }

        public async Task<TableRecordViewModel[]> GetAllTablesAsync(string userId)
        {
            var tableNames = await this.context.TableRecords
                .Where(tr => tr.CreatorId == userId && tr.IsDeleted == false)
                .Select(tr => tr.Name)
                .ToArrayAsync();

            var tableViewModels = new List<TableRecordViewModel>(tableNames.Length);
            foreach(var tableName in tableNames)
            {
                var viewModel = await this.GetByNameAsync(tableName, userId);
                tableViewModels.Add(viewModel);
            }

            return tableViewModels.ToArray();
        }

        public async Task<TableRecordViewModel> GetByNameAsync(string tableName, string userId)
        {
            var tableRecord = await this.context.TableRecords
                .Where(tr => tr.Name == tableName && tr.CreatorId == userId && tr.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (tableRecord == null)
            {
                return null;
            }

            var columnViewModels = (CreateColumnInputModel[])JsonSerializer.Deserialize(tableRecord.Columns, typeof(CreateColumnInputModel[]));

            return new TableRecordViewModel()
            {
                Name = tableRecord.Name,
                CreatedOn = tableRecord.CreatedOn.ToString(),
                ModifiedOn = tableRecord.ModifiedOn.ToString(),
                Columns = columnViewModels
                        .Select(c => new ColumnViewModel()
                        {
                            Name = c.Name,
                            IsRequired = c.IsRequired,
                            IsUnique = c.IsUnique,
                            Type = c.Type.ToString()
                        })
                        .ToArray()
            };
        }

        public Task<bool> IsNameUniqueAsync(string tableName, string userId)
        {
            return this.context.TableRecords
                .AllAsync(tr => tr.InternalName != tableName + userId);
        }

        public bool NameContainsInvalidCharacters(string tableName)
        {
            return tableName.Any(letter => char.IsLetter(letter) == false);
        }
    }
}
