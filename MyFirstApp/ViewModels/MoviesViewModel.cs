using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MyFirstApp.Models;
using MyFirstApp.Services;
using MyFirstApp.DTOs;
using Microsoft.Maui.ApplicationModel;

namespace MyFirstApp.ViewModels
{
    public class MoviesViewModel : INotifyPropertyChanged
    {
        private readonly IMovieSearchService _searchService;

        public ObservableCollection<MovieViewItem> Movies { get; set; } = new ObservableCollection<MovieViewItem>();

        private string _searchQuery;
        private CancellationTokenSource _cancellationTokenSource;

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (_searchQuery != value)
                {
                    _searchQuery = value;
                    OnPropertyChanged();
                    StartSearch(_searchQuery); // Новый метод
                }
            }
        }

        public MoviesViewModel(IMovieSearchService searchService)
        {
            _searchService = searchService;
        }

        private void StartSearch(string query)
        {
            // Отменить предыдущий поиск, если он ещё идёт
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            // Асинхронный запуск нового поиска
            Task.Run(async () =>
            {
                try
                {
                    await SearchMoviesAsync(query, token);
                }
                catch (OperationCanceledException)
                {
                    // Поиск отменён — можно игнорировать
                }
            });
        }

        public async Task LoadMoviesAsync()
        {
            var results = await _searchService.SearchMoviesAsync("");

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Movies.Clear();
                foreach (var dto in results)
                {
                    Movies.Add(new MovieViewItem
                    {
                        Id = dto.Id,
                        Title = dto.Title,
                        Genre = dto.Genre,
                        Poster = dto.Poster,
                        Actors = dto.Actors
                    });
                }
            });
        }

        public async Task SearchMoviesAsync(string query, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(query))
            {
                await LoadMoviesAsync();
                return;
            }

            var allMovies = await _searchService.SearchMoviesAsync("");
            token.ThrowIfCancellationRequested();

            var filteredMovies = allMovies
                .Where(movie =>
                    movie.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    movie.Actors.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    movie.Genre.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Movies.Clear();
                foreach (var dto in filteredMovies)
                {
                    Movies.Add(new MovieViewItem
                    {
                        Id = dto.Id,
                        Title = dto.Title,
                        Genre = dto.Genre,
                        Poster = dto.Poster,
                        Actors = dto.Actors
                    });
                }
            });
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class MovieViewItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Genre { get; set; } = "";
        public string Poster { get; set; } = "";
        public string Actors { get; set; } = "";
    }
}
