using MyFirstApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFirstApp.DTOs;

namespace MyFirstApp.Services
{
    public class MovieSearchService : IMovieSearchService
    {
        private readonly SQLiteAsyncConnection _db;

        public MovieSearchService(IDatabaseService databaseService)
        {
            _db = databaseService.GetConnection();
        }

        public async Task<List<MovieDto>> SearchMoviesAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                query = "";

            query = query.ToLower();

            var matchedMovies = await _db.Table<Movie>()
                .Where(m => m.Title.ToLower().Contains(query) || m.Genre.ToLower().Contains(query))
                .ToListAsync();

            var matchingActors = await _db.Table<Actor>()
                .Where(a => a.Name.ToLower().Contains(query))
                .ToListAsync();

            if (matchingActors.Any())
            {
                var actorIds = matchingActors.Select(a => a.Id).ToList();

                var movieActorLinks = await _db.Table<MovieActor>()
                    .Where(link => actorIds.Contains(link.ActorId))
                    .ToListAsync();

                var movieIds = movieActorLinks.Select(link => link.MovieId).Distinct().ToList();

                var moviesByActor = await _db.Table<Movie>()
                    .Where(m => movieIds.Contains(m.Id))
                    .ToListAsync();

                matchedMovies.AddRange(moviesByActor);
            }

            var uniqueMovies = matchedMovies
                .GroupBy(m => m.Id)
                .Select(g => g.First())
                .ToList();

            var allMovieActorLinks = await _db.Table<MovieActor>().ToListAsync();
            var allActors = await _db.Table<Actor>().ToListAsync();

            var result = uniqueMovies.Select(movie =>
            {
                var actorIds = allMovieActorLinks
                    .Where(link => link.MovieId == movie.Id)
                    .Select(link => link.ActorId);

                var actorNames = allActors
                    .Where(a => actorIds.Contains(a.Id))
                    .Select(a => a.Name);

                return new MovieDto
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Genre = movie.Genre,
                    Poster = movie.Poster,
                    Actors = string.Join(", ", actorNames)
                };
            }).ToList();

            return result;
        }

    }
}
