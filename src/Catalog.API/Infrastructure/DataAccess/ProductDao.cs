using API.Catalog.Application.DataAccess;
using API.Catalog.Application.Dto;
using Dapper;
using Infrastructure.Base.Database;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace API.Catalog.Infrastructure.DataAccess
{
    public class ProductDao : IProductDao
    {
        private readonly QueryConnectionModel _connectionModel;
        private readonly ILogger<ProductDao> _logger;

        private const string ProductTable = "ms_catalog.Products";

        public ProductDao(QueryConnectionModel connectionModel, ILogger<ProductDao> logger)
        {
            _connectionModel = connectionModel;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDto>> GetLatestProductAsync(int limit)
        {
            IEnumerable<ProductDto> products = null;

            try
            {
                using SqlConnection connection = new SqlConnection(_connectionModel.ConnectionString);
                await connection.OpenAsync();

                var query = string.Format("SELECT TOP(@limit) Id, Name, Description, Price FROM {0}", ProductTable);

                products = await connection.QueryAsync<ProductDto>(query, new { Limit = limit });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return products;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByIdsAsync(int[] ids)
        {
            IEnumerable<ProductDto> products = null;

            try
            {
                using SqlConnection connection = new SqlConnection(_connectionModel.ConnectionString);
                await connection.OpenAsync();

                var query = string.Format("SELECT Id, Name, Description, Price FROM {0} WHERE id IN @Ids", ProductTable);

                products = await connection.QueryAsync<ProductDto>(query, new { Table = ProductTable, Ids = ids });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return products;
        }
    }
}
