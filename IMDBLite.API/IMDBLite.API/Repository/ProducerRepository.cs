using IMDBLite.API.Models.DB;
using IMDBLite.API.Repository.Interfaces;
using IMDBLite.API.Settings;
using Microsoft.Extensions.Options;

namespace IMDBLite.API.Repository;

public class ProducerRepository : BaseRepository<Producer>, IProducerRepository
{
    public ProducerRepository(IOptions<ConnectionString> connectionString)
        : base(connectionString.Value.IMDBDB)
    {
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string storedProcedure = "Foundation.usp_DeleteProducer";
        return await ExecuteAsync(storedProcedure, new { ProducerId = id }) > 0;
    }

    public async Task<IEnumerable<Producer>> GetAllAsync()
    {
        const string query = @"
            SELECT 
                [Id],
                [Name],
                [Bio],
                [DOB] AS [DateOfBirth],
                [Sex] AS [Gender]
            FROM [Foundation].[Producers] WITH (NOLOCK)";

        return await GetManyAsync<Producer>(query, new { });
    }

    public async Task<Producer?> GetByIdAsync(int id)
    {
        const string query = @"
            SELECT 
                [Id],
                [Name],
                [Bio],
                [DOB] AS [DateOfBirth],
                [Sex] AS [Gender]
            FROM [Foundation].[Producers] WITH (NOLOCK)
            WHERE [Id] = @Id";

        return await GetAsync<Producer>(query, new { Id = id });
    }

    public async Task<int> CreateAsync(Producer producer)
    {
        const string query = @"
            INSERT INTO [Foundation].[Producers] (
                [Name], 
                [Bio], 
                [DOB], 
                [Sex]
            )
            VALUES (
                @Name, 
                @Bio, 
                @DateOfBirth, 
                @Gender
            );

            SELECT CAST(SCOPE_IDENTITY() AS INT)";

        return await ExecuteScalarAsync<int>(query, producer);
    }

    public async Task<bool> UpdateAsync(int id, Producer producer)
    {
        const string query = @"
            UPDATE [Foundation].[Producers]
            SET 
                [Name] = @Name,
                [Bio] = @Bio,
                [DOB] = @DateOfBirth,
                [Sex] = @Gender
            WHERE [Id] = @Id";

        producer.Id = id;
        return await ExecuteAsync(query, producer) > 0;
    }
}