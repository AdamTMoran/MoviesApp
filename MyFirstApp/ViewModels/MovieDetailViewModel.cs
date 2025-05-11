using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels
{
  public class MovieDetailViewModel
{
    public Movie Movie { get; }
    public ObservableCollection<Actor> Actors { get; } = new();

    private readonly IDatabaseService _databaseService;

    public MovieDetailViewModel(Movie movie, IDatabaseService databaseService)
    {
        Movie = movie;
        _databaseService = databaseService;
        _ = LoadActorsAsync(movie.Id);
    }

    public async Task LoadActorsAsync(int movieId)
    {
        var db = _databaseService.GetConnection();

        var movieActors = await db.Table<MovieActor>()
            .Where(ma => ma.MovieId == movieId)
            .ToListAsync();

        var actorIds = movieActors.Select(ma => ma.ActorId).ToList();
        var allActors = await db.Table<Actor>().ToListAsync();
        var relevantActors = allActors.Where(a => actorIds.Contains(a.Id)).ToList();

        Actors.Clear();
        foreach (var actor in relevantActors)
        {
            Actors.Add(actor);
        }
    }
}
}

