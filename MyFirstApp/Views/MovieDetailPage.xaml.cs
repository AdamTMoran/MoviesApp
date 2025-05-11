using MyFirstApp.Models;
using MyFirstApp.ViewModels;

namespace MyFirstApp.Views
{
    public partial class MovieDetailPage : ContentPage
    {
        public MovieDetailPage(MovieDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel; // Привязываем ViewModel
        }
    }
}
