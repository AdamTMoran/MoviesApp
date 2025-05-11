using MyFirstApp.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstApp.Services
{
    public interface IMovieSearchService
    {
        Task<List<MovieDto>> SearchMoviesAsync(string query);
    }
}
