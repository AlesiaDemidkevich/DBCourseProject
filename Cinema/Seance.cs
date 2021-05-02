using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    public class Seance
    {
        public Seance(string date, string time, string hall, Film film, string capacity) {
            seanceDate = date;
            seanceTime = time;
            seanceHall = hall;
            seanceFilm = film;
            seanceCapacity = capacity;
        }
        public string seanceDate { get; set; }
        public string seanceTime { get; set; }
        public string seanceHall { get; set; }
        public Film seanceFilm { get; set; }
        public string seanceCapacity { get; set; }
    }
}
