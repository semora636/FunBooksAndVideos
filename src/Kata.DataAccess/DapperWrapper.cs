using Dapper;
using System.Data;

namespace Kata.DataAccess
{
    public class DapperWrapper : IDapperWrapper
    {
        public async Task<T?> QueryFirstOrDefaultAsync<T>(IDbConnection connection, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await connection.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public async Task<int> ExecuteScalarAsync<T>(IDbConnection connection, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await connection.ExecuteScalarAsync<int>(sql, param, transaction, commandTimeout, commandType);
        }

        public async Task<int> ExecuteAsync(IDbConnection connection, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await connection.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
        }
    }
}
