using Cinema.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    public class Ticket
    {
        public Ticket(int ID, int number, Seance seance, Place place, string employeeFio )
        {
            this.ID = ID;
            this.seance = seance;
            this.place = place;
            Cost = place.Cost;
            this.employeeFio = employeeFio;
            Number = number;
        }
        public int ID { get; set; }
        public int Number { get; set; }
        public int Cost { get; set; }
        public Seance seance { get; set; }
        public Place place{ get; set; }
        public string employeeFio { get; set; }
    }
}
