using IMDBLite.API.Models.DB;
using IMDBLite.API.Repository.Interfaces;
using IMDBLite.API.Settings;
using Microsoft.Extensions.Options;

namespace IMDBLite.API.Repository;

public class ActorRepository : BaseRepository<Actor>, IActorRepository
{
    public ActorRepository(IOptions<ConnectionString> connectionString)
        : base(connectionString.Value.IMDBDB)
    {
    }

    public async Task<IEnumerable<Actor>> GetAllAsync()
    {
        const string query = @"
                SELECT 
                    [Id],
                    [Name],
                    [Bio],
                    [DOB] AS [DateOfBirth],
                    [Sex] AS [Gender]
                FROM [Foundation].[Actors] WITH (NOLOCK)";

        return await GetAsync(query);
    }

    public async Task<Actor?> GetByIdAsync(int id)
    {
        const string query = @"
                SELECT 
                    [Id],
                    [Name],
                    [Bio],
                    [DOB] AS [DateOfBirth],
                    [Sex] AS [Gender]
                FROM [Foundation].[Actors] WITH (NOLOCK)
                WHERE [Id] = @Id";

        return await GetAsync<Actor>(query, new { Id = id });
    }

    public async Task<int> CreateAsync(Actor actor)
    {
        const string query = @"
                INSERT INTO [Foundation].[Actors] ([Name], [Bio], [DOB], [Sex])
                VALUES (@Name, @Bio, @DateOfBirth, @Gender);

                SELECT CAST(SCOPE_IDENTITY() AS INT)";

        return await ExecuteScalarAsync<int>(query, actor);
    }

    public async Task<bool> UpdateAsync(int id, Actor actor)
    {
        const string query = @"
                UPDATE [Foundation].[Actors]
                SET 
                    [Name] = @Name,
                    [Bio] = @Bio,
                    [DOB] = @DateOfBirth,
                    [Sex] = @Gender
                WHERE [Id] = @Id";

        actor.Id = id;
        return await ExecuteAsync(query, actor) > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string storedProcedure = "Foundation.usp_DeleteActor";
        return await ExecuteAsync(storedProcedure, new { ActorId = id }) > 0;
    }

    public async Task<List<Actor>> GetByIdsAsync(List<int> ids)
    {
        const string query = "SELECT * FROM [Foundation].[Actors] WHERE Id IN @Ids";
        return (await GetManyAsync<Actor>(query, new { Ids = ids })).ToList();
    }
}