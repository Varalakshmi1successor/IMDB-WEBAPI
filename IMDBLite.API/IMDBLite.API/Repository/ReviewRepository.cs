using IMDBLite.API.Models.DB;
using IMDBLite.API.Repository.Interfaces;
using IMDBLite.API.Settings;
using Microsoft.Extensions.Options;

namespace IMDBLite.API.Repository;

public class ReviewRepository : BaseRepository<Review>, IReviewRepository
{
    public ReviewRepository(IOptions<ConnectionString> connectionString)
        : base(connectionString.Value.IMDBDB)
    {
    }

    public async Task<bool> DeleteAsync(int movieId, int id)
    {
        const string query = @"
            DELETE FROM [Foundation].[Reviews]
            WHERE [Id] = @Id AND [MovieId] = @MovieId";

        return await ExecuteAsync(query, new { Id = id, MovieId = movieId }) > 0;
    }

    public async Task<List<Review>> GetAllByMovieIdAsync(int movieId)
    {
        const string query = @"
            SELECT [Id], [Message], [MovieId]
            FROM [Foundation].[Reviews] WITH (NOLOCK)
            WHERE [MovieId] = @MovieId";

        var result = await GetManyAsync<Review>(query, new { MovieId = movieId });
        return result.ToList();
    }

    public async Task<Review?> GetByIdAsync(int movieId, int reviewId)
    {
        const string query = @"
            SELECT [Id], [Message], [MovieId]
            FROM [Foundation].[Reviews] WITH (NOLOCK)
            WHERE [Id] = @ReviewId AND [MovieId] = @MovieId";

        return await GetAsync<Review>(query, new { ReviewId = reviewId, MovieId = movieId });
    }

    public async Task<int> CreateAsync(int movieId, Review review)
    {
        const string query = @"
            INSERT INTO [Foundation].[Reviews] ([Message], [MovieId])
            VALUES (@Message, @MovieId);
            SELECT CAST(SCOPE_IDENTITY() AS INT)";

        review.MovieId = movieId;
        return await ExecuteScalarAsync<int>(query, review);
    }

    public async Task<bool> UpdateAsync(int movieId, int id, Review updatedReview)
    {
        const string query = @"
            UPDATE [Foundation].[Reviews]
            SET [Message] = @Message
            WHERE [Id] = @Id AND [MovieId] = @MovieId";

        updatedReview.Id = id;
        updatedReview.MovieId = movieId;
        return await ExecuteAsync(query, updatedReview) > 0;
    }
}