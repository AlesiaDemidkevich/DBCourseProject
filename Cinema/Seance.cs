using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    class Seance
    {        
        public DateTime seanceDate { get; set; }
        public DateTime seanceTime { get; set; }
        public string seanceHall { get; set; }
        public Film seanceFilm { get; set; }
    }
}
