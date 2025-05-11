using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstApp.Services
{
    public interface IDatabaseService
    {
        Task InitAsync();
        Task SeedDataAsync();
        SQLiteAsyncConnection GetConnection();
    }
}
