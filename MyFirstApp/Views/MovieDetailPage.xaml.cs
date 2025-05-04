using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFirstApp.Models;
using MyFirstApp.Services;
using MyFirstApp.ViewModels;

namespace MyFirstApp.Views
{
    public partial class MovieDetailPage : ContentPage
    {
        public MovieDetailPage(Movie movie)
        {
            InitializeComponent();

            var vm = new MovieDetailViewModel(movie);
            BindingContext = vm;

            // Загрузим актёров вручную
            _ = vm.LoadActorsAsync(movie.Id);
        }
    }

}

