using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Users
{
    public class Employee
    {
        public int ID { get; set; }
        public string name { get; set; }       
        public string secondname { get; set; }
        public string surname { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public int IDEmployee { get; set; }
        public DateTime birthday { get; set; }
        public DateTime dateOfEnrollment { get; set; }
        public string phoneNumber { get; set; }
        public double salary { get; set; }
        public string post { get; set; }
    }
}
