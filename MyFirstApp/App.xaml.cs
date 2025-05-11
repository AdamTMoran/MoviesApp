using MyFirstApp.Services;

namespace MyFirstApp
{
    public partial class App : Application
    {
        public App(MainPage mainPage, IDatabaseService databaseService)
        {
            InitializeComponent();

            // Инициализация и заполнение БД (если нужно)
            Task.Run(async () =>
            {
                await databaseService.InitAsync();
                await databaseService.SeedDataAsync();
            });

            // Важно: используем NavigationPage для переходов
            MainPage = new NavigationPage(mainPage);
        }
    }
}
