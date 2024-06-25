using CrudWithDapperGeneric.Data_Access;
using CrudWithDapperGeneric.Models;
using Dapper;
using System.Data;
using System.Reflection;
using static CrudWithDapperGeneric.Repoistery.Interface.IGenericRepoistery;

namespace CrudWithDapperGeneric.Repoistery.Implementation
{
    public class GenericRepositery<T> :  IGenericRepository<T> where T : class
    {
        private readonly DapperDBContext _context;
        public GenericRepositery(DapperDBContext context)
        {
            _context = context;
        }

        private string GetTableName()
        {
            var tableNameAttribute = typeof(T).GetCustomAttributes<TableNameAttribute>();
            return tableNameAttribute != null ? tableNameAttribute.Name : typeof(T).Name;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using (var connection = _context.CreateConnection())
            {
                var tableName = GetTableName();
                return await connection.QueryAsync<T>($"SELECT * FROM {tableName}");
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var tableName = GetTableName();
                return await connection.QuerySingleOrDefaultAsync<T>($"SELECT * FROM {tableName} WHERE Id = @Id", new { Id = id });
            }
        }

        public async Task<int> AddAsync(T entity)
        {
            using (var connection = _context.CreateConnection())
            {
                var tableName = GetTableName();
                var query = $"INSERT INTO {tableName} ({string.Join(",", GetProperties(entity))}) VALUES ({string.Join(",", GetProperties(entity, "@"))});";
                return await connection.ExecuteAsync(query, entity);
            }
        }

        public async Task<int> UpdateAsync(T entity)
        {
            using (var connection = _context.CreateConnection())
            {
                var tableName = GetTableName();
                var query = $"UPDATE {tableName} SET {string.Join(",", GetProperties(entity, " = @"))} WHERE Id = @Id";
                return await connection.ExecuteAsync(query, entity);
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var tableName = GetTableName();
                return await connection.ExecuteAsync($"DELETE FROM {tableName} WHERE Id = @Id", new { Id = id });
            }
        }

        public async Task<IEnumerable<T>> QueryStoredProcedureAsync(string procedureName, object parameters = null)
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        private IEnumerable<string> GetProperties(T entity, string prefix = "")
        {
            return typeof(T).GetProperties().Select(p => prefix + p.Name);
        }

        public async Task<int> ExecuteStoredProcedureAsync(string procedureName, object parameters = null)
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteScalarAsync<int>(procedureName, parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
