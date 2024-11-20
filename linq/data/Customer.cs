using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQ.Exercises.data
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int Age { get; set; }

        public override string ToString()
        {
            return $"{Name}\t{City}\t{Age}";
        }

    }

    public class CustomerComparer : IEqualityComparer<Customer>
    {
        public bool Equals(Customer x, Customer y)
        {
            return x.Name == y.Name && x.City == y.City;
        }

        public int GetHashCode(Customer obj)
        {
            return (obj.Name + obj.City).GetHashCode();
        }
    }
}
