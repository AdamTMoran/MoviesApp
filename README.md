# MoviesApp
MoviesApp — это мобильное кросс-платформенное приложение, созданное с использованием .NET MAUI. Оно предназначено для отображения списка фильмов и актёров с возможностью поиска. Приложение построено на принципах архитектурной чистоты и использует шаблоны MVVM и Dependency Injection.

## Технологический стек
- **.NET MAUI**: Основной фреймворк для кросс-платформенной мобильной разработки (Android, iOS, Windows).
- **C#**: Основной язык программирования.
- **SQLite**: Встроенная база данных для хранения фильмов и информации об актёрах.
- **Dependency Injection**: Используется для инъекции сервисов и ViewModel через `MauiProgram.cs`.
- **MVVM (Model-View-ViewModel)**: Используется для разделения представления, бизнес-логики и данных.
- **DTO (Data Transfer Objects)**: Для передачи данных между слоями.
- **LINQ**: Применяется для фильтрации и обработки коллекций данных.
## Установка и запуск
1. Клонируйте репозиторий
    git clone https://github.com/yourusername/MyFirstApp.git
    cd MyFirstApp
2. Откройте проект в Visual Studio
Убедитесь, что установлен .NET SDK версии 8+ и рабочая нагрузка .NET MAUI.
3. Запустите приложение
Выберите платформу (Android/iOS/Windows) и нажмите Run (F5) в Visual Studio.
## Основные компоненты
### ViewModel
MoviesViewModel содержит логику отображения и поиска фильмов, работает с IMovieSearchService, использует ObservableCollection и реализует INotifyPropertyChanged.
### Services
Интерфейс IMovieSearchService и его реализация отвечают за поиск фильмов в базе данных.
### Models & DTOs
Movie — основная модель данных.
MovieDto — объект передачи данных.
MovieViewItem — модель для отображения в UI.
## Поиск фильмов
Функция поиска реализована в MoviesViewModel. При вводе текста фильтруются фильмы по названию, с задержкой в 500 мс, чтобы избежать избыточных запросов:
## Главный экран
![image](https://github.com/user-attachments/assets/afa46ed7-3076-4b9f-af36-ed1203d1e784)
## Поиск фильма
![image](https://github.com/user-attachments/assets/7f3b3d41-d249-4c6d-814b-4b175944f778)
## Поиск фильма
![image](https://github.com/user-attachments/assets/02a8a4b9-e959-4d0c-8ff8-31ca02c3e4a5)
## Вкладка с описанием фильма
![image](https://github.com/user-attachments/assets/6285fcf8-6030-4a13-ac18-8dbaf4390642)




