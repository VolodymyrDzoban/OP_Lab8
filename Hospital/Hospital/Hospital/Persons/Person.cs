using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Persons
{
    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public string GetFullName()
        {
            return $"{Name} {Surname}";
        }
    }
}
