using IMDBLite.API.Models;
using IMDBLite.API.Repository.Interfaces;
using IMDBLite.API.Settings;
using Microsoft.Extensions.Options;

namespace IMDBLite.API.Repository;

public class GenreRepository : BaseRepository<Genre>, IGenreRepository
{
    public GenreRepository(IOptions<ConnectionString> connectionString)
        : base(connectionString.Value.IMDBDB)
    {
    }

    public async Task<List<Genre>> GetAllAsync()
    {
        const string query = @"
            SELECT 
                [Id], 
                [Name]
            FROM [Foundation].[Genres] WITH (NOLOCK)";

        var genres = await GetAsync(query);
        return genres.ToList();
    }

    public async Task<Genre?> GetByIdAsync(int id)
    {
        const string query = @"
            SELECT 
                [Id], 
                [Name]
            FROM [Foundation].[Genres] WITH (NOLOCK)
            WHERE [Id] = @Id";

        return await GetAsync<Genre>(query, new { Id = id });
    }

    public async Task<int> CreateAsync(Genre genre)
    {
        const string query = @"
            INSERT INTO [Foundation].[Genres] ([Name])
            VALUES (@Name);

            SELECT CAST(SCOPE_IDENTITY() AS INT)";

        return await ExecuteScalarAsync<int>(query, genre);
    }

    public async Task<bool> UpdateAsync(int id, Genre genre)
    {
        const string query = @"
            UPDATE [Foundation].[Genres]
            SET [Name] = @Name
            WHERE [Id] = @Id";

        genre.Id = id;
        return await ExecuteAsync(query, genre) > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string query = @"
            DELETE FROM [Foundation].[Genres]
            WHERE [Id] = @Id";

        return await ExecuteAsync(query, new { Id = id }) > 0;
    }

    public async Task<List<Genre>> GetByIdsAsync(List<int> ids)
    {
        const string query = @"
        SELECT Id, Name
        FROM [Foundation].[Genres]
        WHERE Id IN @Ids";

        return (await GetManyAsync<Genre>(query, new { Ids = ids })).ToList();
    }
}