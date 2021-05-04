using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
     public class Place
    {
        public Place()
        {

        }
        public Place(int raw, int number, Hall hall, string type, int cost)
        {
            Raw = raw;
            Number = number;
            Hall = hall;
            Type = type;
            Cost = cost;
        }
        public int Raw { get; set; }
        public int Number{ get; set; }
        public Hall Hall { get; set; }
        public int ID { get; set; }
        public string Type { get; set; }
        public int Cost { get; set; }
    }
}
