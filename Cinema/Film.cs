using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    public class Film
    {
        public string name { get; set; }
        public string genre { get; set; }
        public string year { get; set; }
        public string duration { get; set; }
        public string ageLimit { get; set; }
        public DateTime startreleaseDate { get; set; }
        public DateTime endreleaseDate { get; set; }
        public string description { get; set; }

    }
}
