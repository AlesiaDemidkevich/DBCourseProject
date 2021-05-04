using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    public class Film
    {
        public Film() { }

        public Film(string name) {
            Name = name;
        }
        public Film(string name, string duration, string agelimit)
        {
            Name = name;
            Duration = duration;
            AgeLimit = agelimit;
        }
        public Film(int ID,string name, string genre, string year, string duration, string agelimit, string start, string end, byte[] img, string description) {
            this.ID = ID;
            Name = name;
            Genre = genre;
            Year = year;
            Duration = duration;
            AgeLimit = agelimit;
            StartreleaseDate = start;
            EndreleaseDate = end;
            Imagine = img;
            Description = description;
        
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Year { get; set; }
        public string Duration { get; set; }
        public string AgeLimit { get; set; }
        public string StartreleaseDate { get; set; }
        public string EndreleaseDate { get; set; }
        public string Description { get; set; }
        public byte[] Imagine { get; set; }
       
    }
}
