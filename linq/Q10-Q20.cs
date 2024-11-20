using LINQ.Exercises.data;
using LINQ.Exercises.data.LINQTut07.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LINQ.Exercises
{
    public class Q10_Q20
    {
        public static void questions()
        {
            // FilterCustomers();
            // HighestAge();
            // NamesLengthMoreThan4();
            // OldestCustomers();
            //CustomersWIthDictionary();
            // CustomersWIthHighestAge();
            //MergeLists1();
            //NewYorktCustomers();
            //GroupCustomersWithAge();
            NewYorktCustomersOrderd();

        }

        //q11:Find customers who live in either 'Chicago' or 'San Francisco' and are older than 30.
        public static void FilterCustomers()
        {
            var customers = Repository.LoadCustomers();
            var SpesifcCustomers = customers.Where(x => (x.City.Equals("Chicago") || x.City.Equals("San Francisco"))
                                      && x.Age > 30);

            foreach (var customer in SpesifcCustomers)
            {
                Console.WriteLine($"city : {customer.City} ,Name : {customer.Name},Age : {customer.Age}");
            }
        }

        //q12: Find the customer with the highest age.
        public static void HighestAge()
        {
            var customers = Repository.LoadCustomers();
            var highestAge = customers.OrderByDescending(x=>x.Age).First();
            Console.WriteLine($"customer with the highest age. : Name : {highestAge.Name},Age : {highestAge.Age}");
        }

        //q13 : Find customers whose names have more than 4 letters.
        public static void NamesLengthMoreThan4()
        {
            var customers = Repository.LoadCustomers();
            var SpesifcCustomers = customers.Where(x => x.Name.Length >4);

            foreach (var customer in SpesifcCustomers)
            {
                Console.WriteLine($"Name : {customer.Name},names letters : {customer.Name.Length}");
            }
        }

        //q14: Get the top 3 oldest customers who live in Chicago, sorted by age in descending order.
        public static void OldestCustomers()
        {
            var customers = Repository.LoadCustomers();
            var SpesifcCustomers = customers.Where(x => x.City.Equals("Chicago")).
                                    OrderByDescending(x=>x.Age).Take(3).ToList() ;

            foreach (var customer in SpesifcCustomers)
            {
                Console.WriteLine($"Name : {customer.Name},names letters : {customer.Age}");
            }
        }

        //q15:Create a dictionary where the key is the first letter of the city and the value is the average age of customers from that city.
        public static void CustomersWIthDictionary()
        {
            var customers = Repository.LoadCustomers();
            var SpesifcCustomers = customers.GroupBy(c => c.City[0])
                          .ToDictionary(g => g.Key, g => g.Average(c => c.Age)); 

            foreach (var kvp in SpesifcCustomers)
            {
                Console.WriteLine($"Name : {kvp.Key},names letters : {kvp.Value}");
            }
        }

        //q16:  Find the customers who have the highest age from each city.
        public static void CustomersWIthHighestAge()
        {
            var customers = Repository.LoadCustomers();
            var oldestPerCity = customers.GroupBy(c => c.City).
                Select(x=>new { City = x.Key, HighestAge = x.OrderByDescending(a => a.Age).First() }).ToList() ;

            foreach (var customer in oldestPerCity)
            {
                Console.WriteLine($"city : {customer.City},HighestAge: {customer.HighestAge}");
            }
        }

        //q17:Merge two lists of customers (one with a different dataset) and eliminate duplicates based on name and city.
        public static void MergeLists1()
        {
            var customers1 = Repository.LoadCustomers().Randomize(10);
            var customers2 = Repository.LoadCustomers().Randomize(10);

            var grouped = customers1.Concat(customers2).DistinctBy(c => new { c.Name, c.City })
                .GroupBy(x=>x.City).Select(s=>new { City = s.Key, Names = s.Select(x => x.Name).ToList()});
           
            foreach (var group in grouped)
            {
                Console.WriteLine($"City: {group.City}");
                foreach (var name in group.Names)
                {
                    Console.WriteLine($"   Name: {name}");
                }
            }

            //another Soluation
            var grouped1 = customers1.Union(customers2, new CustomerComparer()).ToList();

            foreach (var group in grouped1)
            {
                Console.WriteLine($"City: {group.City} Name: {group.Name}");
            }
        }


        //q18:Check if all customers in New York are older than 25.
        public static void NewYorktCustomers()
        {
            var customers = Repository.LoadCustomers();
            var SpesifcCustomers = customers.Where(x => x.City.Equals("New York")).All(x=>x.Age >25);

                Console.WriteLine($"all customers in New York are older than 25. :{SpesifcCustomers}");
        }


        //q19: Group customers by age ranges (e.g., 20-29, 30-39, etc.) and count how many customers are in each age range.
        public static void GroupCustomersWithAge()
        {
            var customers = Repository.LoadCustomers();
            var groupedByAge = customers.GroupBy(c =>
            {
                if (c.Age >= 20 && c.Age <= 29) return "20-29";
                if (c.Age >= 30 && c.Age <= 39) return "30-39";
                if (c.Age >= 40 && c.Age <= 49) return "40-49";
                if (c.Age >= 50 && c.Age <= 59) return "50-59";
                return "60+";
            }).Select(x=>new {Range= x.Key , Count =x.Count() }).ToList();


            foreach (var group in groupedByAge)
            {
                Console.WriteLine($"Age Range: {group.Range}, Count: {group.Count}");
            }

            // another soluation 
            var ageRanges = customers
                 .GroupBy(c => (c.Age / 10) * 10)
                 .Select(g => new
                 {
                     AgeRange = $"{g.Key}-{g.Key + 9}",
                     Count = g.Count()
                 })
                 .ToList();
           
            foreach (var group in ageRanges)
            {
                Console.WriteLine($"Age Range: {group.AgeRange}, Count: {group.Count}");
            }

        }



        // q10: Get customers who live in either New York or Chicago, and order them by age and then by name alphabetically.
        public static void NewYorktCustomersOrderd()
        {
            var customers = Repository.LoadCustomers();
            var SpesifcCustomers = customers.Where(x => x.City.Equals("New York")).OrderBy(x => x.Age).OrderBy(x=>x.Name);


            foreach (var group in SpesifcCustomers)
            {
                Console.WriteLine($"City: {group.City} , Age: {group.Age} , Name: {group.Name}");
            }
        }


    }
}
