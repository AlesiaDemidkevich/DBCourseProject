using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Users
{
    public class Employee
    {
        public Employee()
        {
        }
        public Employee(string fio, string name, string secondname, string surname, string birthday, string startDay, string phone, string post, string salary)
        {
            FIO = fio;
            Name = name;
            Secondname = secondname;
            Surname = surname;
            Birthday = birthday;
            DateOfEnrollment = startDay;
            PhoneNumber = phone;
            Post = post;
            Salary = salary;
        }

        public Employee(string name, string secondname, string surname)
        {
            Name = name;
            Secondname = secondname;
            Surname = surname;
        }
        public int ID { get; set; }
        public string Name { get; set; }       
        public string Secondname { get; set; }
        public string Surname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int IDEmployee { get; set; }
        public string Birthday { get; set; }
        public string DateOfEnrollment { get; set; }
        public string PhoneNumber { get; set; }
        public string Salary { get; set; }
        public string Post { get; set; }
        public string FIO { get; set; }
    }
}
