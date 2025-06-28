using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using McpServer.DatabaseLogic;
using McpServer.Models;
using ModelContextProtocol.Server;

namespace McpServer.Tools
{
    /// <summary>
    /// Exposes safe, MCP-hosted methods to query product categories from the SalesLT database.
    /// </summary>
    [McpServerToolType]
    public class SqlTool
    {
        private readonly IConnectionStringService _connService;
        private readonly SqlDataAccess _dataAccess;

        public SqlTool(
            IConnectionStringService connService,
            SqlDataAccess dataAccess)
        {
            _connService = connService;
            _dataAccess = dataAccess;
        }

        /// <summary>
        /// Returns all distinct product categories.
        /// </summary>
        [McpServerTool, Description("Returns all product categories.")]
        public async Task<IEnumerable<ProductCategoryModel>> GetProductCategories()
        {
            const string sql = @"
                SELECT DISTINCT
                    ParentProductCategoryID,
                    ProductCategoryID,
                    Name AS ProductCategory
                FROM [SalesLT].[ProductCategory]";

            var conn = await _connService.GetConnectionString();
            return _dataAccess.LoadData<ProductCategoryModel, dynamic>(sql, new { }, conn);
        }

        /// <summary>
        /// Returns product categories filtered by parent category ID.
        /// </summary>
        [McpServerTool, Description("Returns product categories for a specific parent category.")]
        public async Task<IEnumerable<ProductCategoryModel>> GetProductCategoriesByParent(
            [Description("The ID of the parent product category.")] int parentId)
        {
            const string sql = @"
                SELECT DISTINCT
                    ParentProductCategoryID,
                    ProductCategoryID,
                    Name AS ProductCategory
                FROM [SalesLT].[ProductCategory]
                WHERE ParentProductCategoryID = @ParentId";

            var conn = await _connService.GetConnectionString();
            return _dataAccess.LoadData<ProductCategoryModel, dynamic>(sql, new { ParentId = parentId }, conn);
        }
    }
}
