using IMDBLite.API.Models;
using IMDBLite.API.Models.DB;
using IMDBLite.API.Repository.Interfaces;
using IMDBLite.API.Settings;
using Microsoft.Extensions.Options;

namespace IMDBLite.API.Repository;

public class MovieRepository : BaseRepository<Movie>, IMovieRepository
{
    public MovieRepository(IOptions<ConnectionString> connectionString)
        : base(connectionString.Value.IMDBDB)
    {
    }

    public async Task<List<Movie>> GetAllAsync()
    {
        var movies = (await QueryAsync<Movie, Producer, Movie>(
            @"
            SELECT 
                m.Id,
                m.Name,
                m.YearOfRelease,
                m.Plot,
                m.Poster AS CoverImageUrl,
                p.Id AS Id,
                p.Name,
                p.Bio,
                p.Sex AS Gender,
                p.Dob AS DateOfBirth
            FROM Foundation.Movies m
            LEFT JOIN Foundation.Producers p ON m.ProducerId = p.Id
            ",
            (movie, producer) =>
            {
                movie.Producer = producer;
                return movie;
            },
            "Id"
        )).ToList();

        if (!movies.Any())
            return new List<Movie>();

        var movieIds = movies.Select(m => m.Id).ToList();

        var actorMappings = await QueryAsync<int, Actor, Tuple<int, Actor>>(
            @"
            SELECT 
                ma.MovieId AS MovieId,
                a.Id AS Id,
                a.Name,
                a.Sex AS Gender,
                a.Dob AS DateOfBirth,
                a.Bio
            FROM Foundation.Actors_Movies ma
            INNER JOIN Foundation.Actors a ON ma.ActorId = a.Id
            WHERE ma.MovieId IN @MovieIds
            ",
            (movieId, actor) => Tuple.Create(movieId, actor),
            "Id",
            new { MovieIds = movieIds }
        );

        var genreMappings = await QueryAsync<int, Genre, Tuple<int, Genre>>(
            @"
            SELECT 
                mg.MovieId AS MovieId,
                g.Id AS Id,
                g.Name
            FROM Foundation.Genres_Movies mg
            INNER JOIN Foundation.Genres g ON mg.GenreId = g.Id
            WHERE mg.MovieId IN @MovieIds
            ",
            (movieId, genre) => Tuple.Create(movieId, genre),
            "Id",
            new { MovieIds = movieIds }
        );

        var actorDict = actorMappings
            .GroupBy(t => t.Item1)
            .ToDictionary(g => g.Key, g => g.Select(t => t.Item2).ToList());

        var genreDict = genreMappings
            .GroupBy(g => g.Item1)
            .ToDictionary(g => g.Key, g => g.Select(t => t.Item2).ToList());

        foreach (var movie in movies)
        {
            if (actorDict.TryGetValue(movie.Id, out var actors))
                movie.Actors = actors;

            if (genreDict.TryGetValue(movie.Id, out var genres))
                movie.Genres = genres;
        }

        return movies;
    }

    public async Task<List<Movie>> GetByYearAsync(int year)
    {
        var movies = (await QueryAsync<Movie, Producer, Movie>(
            @"
            SELECT 
                m.Id,
                m.Name,
                m.YearOfRelease,
                m.Plot,
                m.Poster AS CoverImageUrl,
                p.Id AS Id,
                p.Name,
                p.Bio,
                p.Sex AS Gender,
                p.Dob AS DateOfBirth
            FROM Foundation.Movies m
            LEFT JOIN Foundation.Producers p ON m.ProducerId = p.Id
            WHERE m.YearOfRelease = @Year
            ",
            (movie, producer) =>
            {
                movie.Producer = producer;
                return movie;
            },
            "Id",
            new { Year = year }
        )).ToList();

        if (!movies.Any())
            return new List<Movie>();

        var movieIds = movies.Select(m => m.Id).ToList();

        var actorMappings = await QueryAsync<int, Actor, Tuple<int, Actor>>(
            @"
            SELECT 
                ma.MovieId AS MovieId,
                a.Id AS Id,
                a.Name,
                a.Sex AS Gender,
                a.Dob AS DateOfBirth,
                a.Bio
            FROM Foundation.Actors_Movies ma
            INNER JOIN Foundation.Actors a ON ma.ActorId = a.Id
            WHERE ma.MovieId IN @MovieIds
            ",
            (movieId, actor) => Tuple.Create(movieId, actor),
            "Id",
            new { MovieIds = movieIds }
        );

        var genreMappings = await QueryAsync<int, Genre, Tuple<int, Genre>>(
            @"
            SELECT 
                mg.MovieId AS MovieId,
                g.Id AS Id,
                g.Name
            FROM Foundation.Genres_Movies mg
            INNER JOIN Foundation.Genres g ON mg.GenreId = g.Id
            WHERE mg.MovieId IN @MovieIds
            ",
            (movieId, genre) => Tuple.Create(movieId, genre),
            "Id",
            new { MovieIds = movieIds }
        );

        var actorDict = actorMappings
            .GroupBy(t => t.Item1)
            .ToDictionary(g => g.Key, g => g.Select(t => t.Item2).ToList());

        var genreDict = genreMappings
            .GroupBy(g => g.Item1)
            .ToDictionary(g => g.Key, g => g.Select(t => t.Item2).ToList());

        foreach (var movie in movies)
        {
            if (actorDict.TryGetValue(movie.Id, out var actors))
                movie.Actors = actors;

            if (genreDict.TryGetValue(movie.Id, out var genres))
                movie.Genres = genres;
        }

        return movies;
    }

    public async Task<Movie?> GetByIdAsync(int id)
    {
        var movies = (await QueryAsync<Movie, Producer, Movie>(
            @"
            SELECT 
                m.Id,
                m.Name,
                m.YearOfRelease,
                m.Plot,
                m.Poster AS CoverImageUrl,
                p.Id AS Id,
                p.Name,
                p.Bio,
                p.Sex AS Gender,
                p.Dob AS DateOfBirth
            FROM Foundation.Movies m
            LEFT JOIN Foundation.Producers p ON m.ProducerId = p.Id
            WHERE m.Id = @Id
            ",
            (movie, producer) =>
            {
                movie.Producer = producer;
                return movie;
            },
            "Id",
            new { Id = id }
        )).ToList();

        var movie = movies.FirstOrDefault();
        if (movie == null)
            return null;

        var actorMappings = await QueryAsync<int, Actor, Tuple<int, Actor>>(
            @"
            SELECT 
                ma.MovieId AS MovieId,
                a.Id AS Id,
                a.Name,
                a.Sex AS Gender,
                a.Dob AS DateOfBirth,
                a.Bio
            FROM Foundation.Actors_Movies ma
            INNER JOIN Foundation.Actors a ON ma.ActorId = a.Id
            WHERE ma.MovieId = @MovieId
            ",
            (movieId, actor) => Tuple.Create(movieId, actor),
            "Id",
            new { MovieId = id }
        );

        var genreMappings = await QueryAsync<int, Genre, Tuple<int, Genre>>(
            @"
            SELECT 
                mg.MovieId AS MovieId,
                g.Id AS Id,
                g.Name
            FROM Foundation.Genres_Movies mg
            INNER JOIN Foundation.Genres g ON mg.GenreId = g.Id
            WHERE mg.MovieId = @MovieId
            ",
            (movieId, genre) => Tuple.Create(movieId, genre),
            "Id",
            new { MovieId = id }
        );

        movie.Actors = actorMappings.Select(t => t.Item2).ToList();
        movie.Genres = genreMappings.Select(t => t.Item2).ToList();

        return movie;
    }

    public async Task<int> CreateAsync(Movie movie)
    {
        var actorIds = string.Join(",", movie.Actors.Select(a => a.Id));
        var genreIds = string.Join(",", movie.Genres.Select(g => g.Id));

        const string query = @"
            EXEC Foundation.usp_AddMovie  
                @Name,  
                @YearOfRelease,  
                @Plot,  
                @Poster,  
                @ProducerId,  
                @ActorIds,  
                @GenreIds
        ";

        return await ExecuteScalarAsync<int>(query, new
        {
            movie.Name,
            movie.YearOfRelease,
            movie.Plot,
            Poster = movie.CoverImageUrl,
            ProducerId = movie.Producer.Id,
            ActorIds = actorIds,
            GenreIds = genreIds
        });
    }

    public async Task<bool> UpdateAsync(int id, Movie updatedMovie)
    {
        var actorIds = string.Join(",", updatedMovie.Actors.Select(a => a.Id));
        var genreIds = string.Join(",", updatedMovie.Genres.Select(g => g.Id));

        const string query = @"
            EXEC Foundation.usp_UpdateMovie  
                @MovieId,  
                @Name,  
                @YearOfRelease,  
                @Plot,  
                @Poster,  
                @ProducerId,  
                @ActorIds,  
                @GenreIds
        ";

        return await ExecuteAsync(query, new
        {
            MovieId = id,
            updatedMovie.Name,
            updatedMovie.YearOfRelease,
            updatedMovie.Plot,
            Poster = updatedMovie.CoverImageUrl,
            ProducerId = updatedMovie.Producer.Id,
            ActorIds = actorIds,
            GenreIds = genreIds
        }) > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string query = "EXEC Foundation.usp_DeleteMovie @MovieId";
        return await ExecuteAsync(query, new { MovieId = id }) > 0;
    }
}