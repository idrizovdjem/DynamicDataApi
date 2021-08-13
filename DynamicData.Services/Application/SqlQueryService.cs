using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DynamicData.Data;
using DynamicData.Services.Application.Contracts;
using System.Collections.Generic;
using DynamicData.DTOs.Column;

namespace DynamicData.Services.Application
{
    public class SqlQueryService : ISqlQueryService
    {
        private readonly ApplicationDbContext context;

        public SqlQueryService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task CreateTableAsync(string sqlQuery)
        {
            await this.context.Database.ExecuteSqlRawAsync(sqlQuery);
        }
    }
}
