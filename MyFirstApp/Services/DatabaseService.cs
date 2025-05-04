using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using MyFirstApp.Models;

namespace MyFirstApp.Services
{


    public static class DatabaseService
    {
        private static SQLiteAsyncConnection _database;

        // Метод инициализации базы
        public static async Task InitAsync()
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

        public static SQLiteAsyncConnection GetConnection()
        {
            return _database;
        }



        public static async Task SeedDataAsync()
        {
            var db = GetConnection();

            // Очистим таблицы (если нужно начать с нуля)
            await db.DeleteAllAsync<MovieActor>();
            await db.DeleteAllAsync<Movie>();
            await db.DeleteAllAsync<Actor>();

            // 1. Добавим фильмы
            var movies = new List<Movie>
    {
        new Movie { Title = "Inception", Genre = "Sci-Fi" },
        new Movie { Title = "Titanic", Genre = "Drama" },
        new Movie { Title = "The Dark Knight", Genre = "Action" }
    };
            await db.InsertAllAsync(movies);

            // Обновим список с присвоенными ID
            movies = await db.Table<Movie>().ToListAsync();

            // 2. Добавим актёров
            var actors = new List<Actor>
    {
        new Actor { Name = "Leonardo DiCaprio" },
        new Actor { Name = "Joseph Gordon-Levitt" },
        new Actor { Name = "Kate Winslet" },
        new Actor { Name = "Christian Bale" },
        new Actor { Name = "Heath Ledger" }
    };
            await db.InsertAllAsync(actors);

            // Обновим список с ID
            actors = await db.Table<Actor>().ToListAsync();

            // 3. Связи фильм ↔ актёры
            var movieActorLinks = new List<MovieActor>
    {
        // Inception: DiCaprio, Gordon-Levitt
        new MovieActor { MovieId = movies.First(m => m.Title == "Inception").Id, ActorId = actors.First(a => a.Name == "Leonardo DiCaprio").Id },
        new MovieActor { MovieId = movies.First(m => m.Title == "Inception").Id, ActorId = actors.First(a => a.Name == "Joseph Gordon-Levitt").Id },

        // Titanic: DiCaprio, Winslet
        new MovieActor { MovieId = movies.First(m => m.Title == "Titanic").Id, ActorId = actors.First(a => a.Name == "Leonardo DiCaprio").Id },
        new MovieActor { MovieId = movies.First(m => m.Title == "Titanic").Id, ActorId = actors.First(a => a.Name == "Kate Winslet").Id },

        // The Dark Knight: Bale, Ledger
        new MovieActor { MovieId = movies.First(m => m.Title == "The Dark Knight").Id, ActorId = actors.First(a => a.Name == "Christian Bale").Id },
        new MovieActor { MovieId = movies.First(m => m.Title == "The Dark Knight").Id, ActorId = actors.First(a => a.Name == "Heath Ledger").Id }
    };
            await db.InsertAllAsync(movieActorLinks);
        }

    }





}

