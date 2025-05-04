using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MyFirstApp.Models;
using MyFirstApp.Services;


namespace MyFirstApp.ViewModels
{
    public class MoviesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Movie> Movies { get; set; } = new();

        private string searchQuery;
        public string SearchQuery
        {
            get => searchQuery;
            set
            {
                if (searchQuery != value)
                {
                    searchQuery = value;
                    OnPropertyChanged();
                    _ = SearchMoviesAsync();  // Мы вызываем асинхронный метод поиска фильмов
                }
            }
        }

        // Метод загрузки всех фильмов
        public async Task LoadMoviesAsync()
        {
            var db = DatabaseService.GetConnection();
            var moviesFromDb = await db.Table<Movie>().ToListAsync();

            Movies.Clear();
            foreach (var movie in moviesFromDb)
                Movies.Add(movie);
        }

        // Метод для поиска фильмов
        public async Task SearchMoviesAsync()
        {
            var db = DatabaseService.GetConnection();
            var query = searchQuery?.ToLower() ?? "";

            // 1. Поиск фильмов по названию или жанру
            var movieResults = await db.Table<Movie>()
                .Where(m => m.Title.ToLower().Contains(query)
                            || m.Genre.ToLower().Contains(query))
                .ToListAsync();

            // 2. Поиск фильмов по актёрам
            var actorMatches = await db.Table<Actor>()
                .Where(a => a.Name.ToLower().Contains(query))
                .ToListAsync();

            if (actorMatches.Any())
            {
                // Получим MovieId для каждого найденного актёра
                var actorIds = actorMatches.Select(a => a.Id).ToList();

                var movieActorLinks = await db.Table<MovieActor>()
                    .Where(link => actorIds.Contains(link.ActorId))
                    .ToListAsync();

                var movieIdsFromActors = movieActorLinks.Select(link => link.MovieId);

                // Добавим фильмы, связанные с актёрами
                var moviesByActors = await db.Table<Movie>()
                    .Where(m => movieIdsFromActors.Contains(m.Id))
                    .ToListAsync();

                // Объединим результаты
                movieResults.AddRange(moviesByActors);
            }

            // Удалим дубликаты (если фильм есть в обоих результатах)
            var distinct = movieResults
                .GroupBy(m => m.Id)
                .Select(g => g.First())
                .ToList();

            Movies.Clear();
            foreach (var movie in distinct)
                Movies.Add(movie);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
