using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels
{
    public class MovieDetailViewModel
    {
        public Movie Movie { get; }
        public ObservableCollection<Actor> Actors { get; } = new();

        public MovieDetailViewModel(Movie movie)
        {
            Movie = movie;
            LoadActorsAsync(movie.Id);  // Загружаем актеров для выбранного фильма
        }

        public async Task LoadActorsAsync(int movieId)
        {
            var db = DatabaseService.GetConnection();

            var movieActors = await db.Table<MovieActor>()
                .Where(ma => ma.MovieId == movieId)
                .ToListAsync();

            var actorIds = movieActors.Select(ma => ma.ActorId).ToList();

            var allActors = await db.Table<Actor>().ToListAsync();
            var relevantActors = allActors.Where(a => actorIds.Contains(a.Id)).ToList();

            System.Diagnostics.Debug.WriteLine($"Найдено актёров: {relevantActors.Count}");

            Actors.Clear();
            foreach (var actor in relevantActors)
            {
                System.Diagnostics.Debug.WriteLine($"Добавляется актёр: {actor.Name}");
                Actors.Add(actor);
            }
        }

    }
}

