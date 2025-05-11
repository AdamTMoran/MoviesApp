using MyFirstApp.Models;
using MyFirstApp.Services;
using MyFirstApp.ViewModels;
using MyFirstApp.Views;
using System.Linq;

namespace MyFirstApp
{
    public partial class MainPage : ContentPage
    {
        private bool isLoaded = false;
        private readonly IDatabaseService _databaseService;

        public MainPage(MoviesViewModel viewModel, IDatabaseService databaseService)
        {
            InitializeComponent();
            BindingContext = viewModel;  // Привязываем ViewModel к BindingContext
            _databaseService = databaseService;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!isLoaded && BindingContext is MoviesViewModel vm)
            {
                isLoaded = true;
                await vm.LoadMoviesAsync();
            }
        }

        // Обработчик события для выбора фильма
        private async void OnMovieTapped(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is MovieViewItem selectedMovie)
            {
                // Создание ViewModel для страницы деталей фильма
                var movieDetailViewModel = new MovieDetailViewModel(new Movie { Id = selectedMovie.Id, Title = selectedMovie.Title }, _databaseService);

                // Создание страницы с деталями фильма
                var page = new MovieDetailPage(movieDetailViewModel);

                // Переход на страницу деталей фильма
                await Navigation.PushAsync(page);

                // Снимаем выделение с выбранного элемента в CollectionView
                ((CollectionView)sender).SelectedItem = null;
            }
        }
    }
}
