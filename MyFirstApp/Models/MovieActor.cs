using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstApp.Models
{
    public class MovieActor
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int MovieId { get; set; }
        public int ActorId { get; set; }
    }
}
