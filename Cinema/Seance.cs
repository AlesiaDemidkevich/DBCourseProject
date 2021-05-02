using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    public class Seance
    {
        public Seance(int ID,string date,DateTime date2, string time, string hall, Film film, string capacity) {
            seanceID = ID;
            seanceDate = date;
            seanceDate2 = date2;
            seanceTime = time;
            seanceHall = hall;
            seanceFilm = film;
            seanceCapacity = capacity;
        }
        public int seanceID { get; set; }
        public string seanceDate { get; set; }
        public DateTime seanceDate2 { get; set; }
        public string seanceTime { get; set; }
        public string seanceHall { get; set; }
        public Film seanceFilm { get; set; }
        public string seanceCapacity { get; set; }
    }
}

