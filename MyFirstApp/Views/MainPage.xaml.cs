using MyFirstApp.Models;
using MyFirstApp.Services;
using MyFirstApp.ViewModels;
using MyFirstApp.Views;
namespace MyFirstApp
{
    public partial class MainPage : ContentPage
    {
        private bool isLoaded = false;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MoviesViewModel();
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
            if (e.CurrentSelection.FirstOrDefault() is Movie selectedMovie)
            {

                await Navigation.PushAsync(new MovieDetailPage(selectedMovie));
                ((CollectionView)sender).SelectedItem = null; // ❗️Сброс выбора
            }
        }
    }

}

