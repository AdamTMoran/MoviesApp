using Microsoft.Extensions.Logging;
using MyFirstApp.Services;
using MyFirstApp.Models;
using MyFirstApp.Views;
using MyFirstApp.ViewModels;
namespace MyFirstApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<IMovieSearchService, MovieSearchService>();
            builder.Services.AddTransient<MoviesViewModel>();
            builder.Services.AddTransient<MainPage>();
            Task.Run(async () =>
            {
                await DatabaseService.InitAsync();
                await DatabaseService.SeedDataAsync();
            }).Wait();
            return builder.Build();
        }
    }
}
