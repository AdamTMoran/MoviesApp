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
using MyFirstApp.DTOs;

namespace MyFirstApp.ViewModels
{
    public class MoviesViewModel : INotifyPropertyChanged
    {
        private readonly IMovieSearchService _searchService;
        public ObservableCollection<MovieViewItem> Movies { get; set; } = new();
       
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

        public MoviesViewModel(IMovieSearchService searchService)
        {
            _searchService = searchService;
        }
        // Метод загрузки всех фильмов
        public async Task LoadMoviesAsync()
        {
            var results = await _searchService.SearchMoviesAsync(""); // пустой запрос = все фильмы

            Movies.Clear();

            foreach (var dto in results)
            {
                // Преобразование MovieDto в MovieViewItem
                Movies.Add(new MovieViewItem
                {
                    Id = dto.Id,
                    Title = dto.Title,
                    Genre = dto.Genre,
                    Poster = dto.Poster,
                    Actors = dto.Actors
                });
            }
        }

        // Метод для поиска фильмов
        public async Task SearchMoviesAsync()
        {
            var results = await _searchService.SearchMoviesAsync(searchQuery);

            Movies.Clear();

            foreach (var dto in results)
            {
                // Преобразование MovieDto в MovieViewItem
                Movies.Add(new MovieViewItem
                {
                    Id = dto.Id,
                    Title = dto.Title,
                    Genre = dto.Genre,
                    Poster = dto.Poster,
                    Actors = dto.Actors
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    // Вспомогательный класс для отображения фильма в UI
    public class MovieViewItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Genre { get; set; } = "";
        public string Poster { get; set; } = "";
        public string Actors { get; set; } = ""; // Строка с именами актёров
    }
}
