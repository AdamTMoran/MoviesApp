using MyFirstApp.Services;

namespace MyFirstApp
{
    public partial class App : Application
    {
        public App(MainPage mainPage)
        {
            InitializeComponent();

            Task.Run(async () =>
            {
                await DatabaseService.InitAsync();
                await DatabaseService.SeedDataAsync();
            });

            // Это ВАЖНО: для навигации нужна оболочка NavigationPage
            MainPage = new NavigationPage(mainPage);
        }

    }
}