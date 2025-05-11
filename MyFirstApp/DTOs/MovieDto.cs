using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstApp.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Genre { get; set; } = "";

        public string Poster { get; set; } = "";
        public string Actors { get; set; } = ""; // Строка с именами актёров
    }
}
