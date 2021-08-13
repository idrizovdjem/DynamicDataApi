using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; 
using DynamicData.Data;
using DynamicData.DTOs.Table;
using DynamicData.Data.Models;
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

        public async Task CreateRecordAsync(string tableName, string userId)
        {
            var tableRecord = new TableRecord()
            {
                Name = tableName,
                CreatorId = userId,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false
            };

            await this.context.TableRecords.AddAsync(tableRecord);
            await this.context.SaveChangesAsync();
        }

        public string GenerateSqlQuery(CreateTableInputModel input, string userId)
        {
            var tableName = input.Name + userId.Replace("-", string.Empty);
            var columnsSqlQuery = this.columnsService.GenerateSqlQuery(input.Columns, input.Name);
            return $"CREATE TABLE {tableName}({columnsSqlQuery})";
        }

        public async Task<TableRecordViewModel[]> GetAllTablesAsync(string userId)
        {
            return await this.context.TableRecords
                .Where(tr => tr.CreatorId == userId && tr.IsDeleted == false)
                .Select(tr => new TableRecordViewModel()
                {
                    Name = tr.Name,
                    CreatedOn = tr.CreatedOn.ToShortDateString(),
                    ModifiedOn = tr.ModifiedOn.ToShortDateString()
                })
                .ToArrayAsync();
        }

        public Task<bool> IsNameUniqueAsync(string tableName, string userId)
        {
            return this.context.TableRecords
                .AllAsync(tr => tr.Name != tableName + userId);
        }

        public bool NameContainsInvalidCharacters(string tableName)
        {
            return tableName.Any(letter => char.IsLetter(letter) == false);
        }
    }
}
