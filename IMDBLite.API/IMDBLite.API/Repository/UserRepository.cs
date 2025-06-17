using IMDBLite.API.Models.DB;
using IMDBLite.API.Repository.Interfaces;
using IMDBLite.API.Settings;
using Microsoft.Extensions.Options;

namespace IMDBLite.API.Repository;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(IOptions<ConnectionString> connectionString)
        : base(connectionString.Value.IMDBDB)
    {
    }

    public async Task DeleteAsync(int id)
    {
        const string query = @"
            DELETE FROM [Foundation].[Users]
            WHERE [Id] = @Id";

        await ExecuteAsync(query, new { Id = id });
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        const string query = @"
            SELECT 
                [Id], 
                [FirstName], 
                [LastName], 
                [Email], 
                [Password]
            FROM [Foundation].[Users] WITH (NOLOCK)";

        return await GetManyAsync<User>(query, null);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        const string query = @"
            SELECT 
                [Id], 
                [FirstName], 
                [LastName], 
                [Email], 
                [Password]
            FROM [Foundation].[Users] WITH (NOLOCK)
            WHERE [Id] = @Id";

        return await GetAsync<User>(query, new { Id = id });
    }

    public async Task AddAsync(User user)
    {
        const string query = @"
            INSERT INTO [Foundation].[Users] (
                [FirstName], 
                [LastName], 
                [Email], 
                [Password]
            )
            VALUES (
                @FirstName, 
                @LastName, 
                @Email, 
                @Password
            );

            SELECT CAST(SCOPE_IDENTITY() AS INT)";

        user.Id = await ExecuteScalarAsync<int>(query, user);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        const string query = @"
            SELECT 
                [Id], 
                [FirstName], 
                [LastName], 
                [Email], 
                [Password]
            FROM [Foundation].[Users] WITH (NOLOCK)
            WHERE [Email] = @Email";

        return await GetAsync<User>(query, new { Email = email });
    }

    public async Task<User?> UpdateAsync(int id, User updatedUser)
    {
        const string query = @"
            UPDATE [Foundation].[Users]
            SET 
                [FirstName] = @FirstName,
                [LastName] = @LastName,
                [Email] = @Email,
                [Password] = @Password
            WHERE [Id] = @Id";

        updatedUser.Id = id;
        var affected = await ExecuteAsync(query, updatedUser);
        return affected > 0 ? await GetByIdAsync(id) : null;
    }
}