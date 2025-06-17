using Dapper;
using Microsoft.Data.SqlClient;

namespace IMDBLite.API.Repository;

public class BaseRepository<T> where T : class
{
    private readonly string _connectionString;

    public BaseRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<T>> GetAsync(string query)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<T>(query);
    }

    public async Task<T> GetAsync<T>(string query, object parameters)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<T>(query, parameters);
    }

    public async Task<IEnumerable<T>> GetManyAsync<T>(string query, object parameters)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<T>(query, parameters);
    }

    public async Task<int> ExecuteAsync(string sql, object? param = null)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.ExecuteAsync(sql, param);
    }

    public async Task<TResult> ExecuteScalarAsync<TResult>(string sql, object? param = null)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.ExecuteScalarAsync<TResult>(sql, param);
    }

    protected async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(
        string sql,
        Func<TFirst, TSecond, TReturn> map,
        string splitOn,
        object? param = null)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync(
            sql, map, param, splitOn: splitOn);
    }
}