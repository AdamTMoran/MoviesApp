using SQLite;
using MyFirstApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace MyFirstApp.Services
{
    public class DatabaseService : IDatabaseService
    {
        private SQLiteAsyncConnection _database;

        public async Task InitAsync()
        {
            if (_database != null)
                return;

            var dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "movies.db");

            _database = new SQLiteAsyncConnection(dbPath);
            await _database.CreateTableAsync<Movie>();
            await _database.CreateTableAsync<Actor>();
            await _database.CreateTableAsync<MovieActor>();
        }

        public SQLiteAsyncConnection GetConnection()
        {
            if (_database == null)
                throw new InvalidOperationException("База данных не инициализирована. Вызовите InitAsync() сначала.");
            return _database;
        }

        public async Task SeedDataAsync()
        {
            var db = GetConnection();

            await db.DeleteAllAsync<MovieActor>();
            await db.DeleteAllAsync<Movie>();
            await db.DeleteAllAsync<Actor>();

            var movies = new List<Movie>
            {
                new Movie { Title = "Inception", Genre = "Sci-Fi", Poster = "inception.jpg" },
                new Movie { Title = "Titanic", Genre = "Drama", Poster = "titanic.jpg" },
                new Movie { Title = "The Dark Knight", Genre = "Action", Poster = "dark_knight.jpg" }
            };

            await db.InsertAllAsync(movies);
            movies = await db.Table<Movie>().ToListAsync();

            var actors = new List<Actor>
            {
                new Actor { Name = "Leonardo DiCaprio" },
                new Actor { Name = "Joseph Gordon-Levitt" },
                new Actor { Name = "Kate Winslet" },
                new Actor { Name = "Christian Bale" },
                new Actor { Name = "Heath Ledger" }
            };

            await db.InsertAllAsync(actors);
            actors = await db.Table<Actor>().ToListAsync();

            var movieActorLinks = new List<MovieActor>
            {
                new MovieActor { MovieId = movies.First(m => m.Title == "Inception").Id, ActorId = actors.First(a => a.Name == "Leonardo DiCaprio").Id },
                new MovieActor { MovieId = movies.First(m => m.Title == "Inception").Id, ActorId = actors.First(a => a.Name == "Joseph Gordon-Levitt").Id },

                new MovieActor { MovieId = movies.First(m => m.Title == "Titanic").Id, ActorId = actors.First(a => a.Name == "Leonardo DiCaprio").Id },
                new MovieActor { MovieId = movies.First(m => m.Title == "Titanic").Id, ActorId = actors.First(a => a.Name == "Kate Winslet").Id },

                new MovieActor { MovieId = movies.First(m => m.Title == "The Dark Knight").Id, ActorId = actors.First(a => a.Name == "Christian Bale").Id },
                new MovieActor { MovieId = movies.First(m => m.Title == "The Dark Knight").Id, ActorId = actors.First(a => a.Name == "Heath Ledger").Id }
            };

            await db.InsertAllAsync(movieActorLinks);
        }
    }
}
