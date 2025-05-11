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

            // Регистрация сервисов
            builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
            builder.Services.AddSingleton<IMovieSearchService, MovieSearchService>();

            // Регистрация ViewModels и страниц
            builder.Services.AddTransient<MoviesViewModel>();
            builder.Services.AddTransient<MainPage>();

            var app = builder.Build();

            // Инициализация базы данных и заполнение данных
            Task.Run(async () =>
            {
                var dbService = app.Services.GetRequiredService<IDatabaseService>();
                await dbService.InitAsync();
                await dbService.SeedDataAsync();
            }).Wait();

            return app;
        }

    }
}
