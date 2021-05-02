using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    public class Hall
    {
        public Hall() { 

        }
        public Hall(string name, string capacity)
        {
            Name = name;
            Capacity = capacity;
        }
        public string Name { get; set; }
        public string Capacity { get; set; }
    }
}
