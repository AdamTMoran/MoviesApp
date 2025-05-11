using MyFirstApp.Models;
using MyFirstApp.Services;
using MyFirstApp.ViewModels;
using MyFirstApp.Views;
namespace MyFirstApp
{
    public partial class MainPage : ContentPage
    {
        private bool isLoaded = false;

        public MainPage(MoviesViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
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

        private async void OnMovieTapped(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is MovieViewItem selectedMovieViewItem)
            {
                // Создаём Movie на основе данных из MovieViewItem
                var movie = new Movie
                {
                    Id = selectedMovieViewItem.Id,
                    Title = selectedMovieViewItem.Title,
                    Genre = selectedMovieViewItem.Genre,
                    Poster = selectedMovieViewItem.Poster
                };

                // Навигация на детальную страницу фильма
                await Navigation.PushAsync(new MovieDetailPage(movie));
                ((CollectionView)sender).SelectedItem = null; // Сброс выбора
            }
        }
    }

}

