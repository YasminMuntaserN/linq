using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;
using System.Xml.Linq;
using LINQ.Exercises.data.LINQTut07.Shared;

namespace LINQ.Exercises
{
    public class Q1_Q10
    {
        public static void questions()
        {
            // EvenNumbers();
            // CointainA();
            //NullOrEmpty();
            //CitiesWithC();
            //customersWithCity();
            // youngestCustomer();
            //SortCustomer();
            //SortCustomerByCity();
            //CustomerByAge();
            AverageAge();
        }

        //q1 :Retrieve all even numbers from a list of integers.
        public static void EvenNumbers()
        {
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var evenNum = numbers.Where(x => x % 2 == 0).ToList();

            Console.WriteLine(string.Join(" , " , evenNum));
        }



        //q2 :Find all strings in a list that contain the letter 'a'.
        public static void CointainA()
        {
            var words = new List<string> { "apple", "banana", "cherry", "date", "fig" };

            var withA = words.Where(x => x.Contains("a")).ToList();

            Console.WriteLine(string.Join(" , ", withA));
        }


        //q3 : Filter out null or empty strings from a list of names.
        public static void NullOrEmpty()
        {
            var names = new List<string> { "John", "", "Jane", null, "Paul" };


            var notNul = names.Where(x => !string.IsNullOrEmpty(x)).ToList();

            Console.WriteLine(string.Join(" , ", notNul));
        }
        //q4 :Get all cities from a list where the name starts with 'C'.
        public static void CitiesWithC()
        {
            var cities = new List<string> { "Chicago", "New York", "Cleveland", "Dallas" };

            var citiesWithc = cities.Where(x => x.StartsWith("C")).ToList();

            Console.WriteLine(string.Join(" , ", citiesWithc));
        }

        //q5 :Group customers by their city and count how many customers are in each city.

        public static void customersWithCity()
        {
            var customers = Repository.LoadCustomers();

            var CustomersInCity = customers.GroupBy(x => x.City).
                                  Select(g => new { City = g.Key, Count = g.Count() }).ToList();


            foreach (var count in CustomersInCity)
            {
                Console.WriteLine($"city : {count.City}  ,Count : {count.Count}");
            }
        }

        //q6: Find the youngest customer from each city.

        public static void youngestCustomer()
        {
            var customers = Repository.LoadCustomers();
            var CustomersInCity = customers.GroupBy(x => x.City).
                               Select(y => new { City = y.Key,
                                                 Age = y.OrderBy(x => x.Age).First().Age }).ToList();
            foreach (var count in CustomersInCity)
            {
                Console.WriteLine($"city : {count.City}  ,Age : {count.Age}");
            }
        }

        //q7:  Sort the customers alphabetically by their names.
        public static void SortCustomer()
        {
            var customers = Repository.LoadCustomers();
            var CustomersInCity = customers.OrderBy(x=>x.Name);
            foreach (var customer in CustomersInCity)
            {
                Console.WriteLine($"city : {customer.Name}");
            }
        }

        //q8: Find customers from Chicago or New York.
        public static void SortCustomerByCity()
        {
            var customers = Repository.LoadCustomers();
            var CustomersInCity = customers.Where(x=>x.City.Equals("Chicago") || x.City.Equals("New York") ).ToList();
            foreach (var customer in CustomersInCity)
            {
                Console.WriteLine($"city : {customer.City} ,Name : {customer.Name}");
            }
        }

        //q9:Get the  customers who are older than 30 and live in Chicago.
        public static void CustomerByAge()
        {
            var customers = Repository.LoadCustomers();
            var CustomersInCity = customers.Where(x => x.City.Equals("Chicago") && x.Age>30).ToList();
            foreach (var customer in CustomersInCity)
            {
                Console.WriteLine($"city : {customer.City} ,Name : {customer.Name},Age : {customer.Age}");
            }
        }

        //q10: Get the average age of customers from New York.
        public static void AverageAge()
        {
            var customers = Repository.LoadCustomers();
            var avg = customers.Where(x => x.City.Equals("New York")).Average(x => x.Age);

                Console.WriteLine($"average age of customers from New York : {avg}");
        }
    }
}
