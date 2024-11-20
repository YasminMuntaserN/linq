using LINQ.Exercises.data;
using LINQ.Exercises.data.LINQTut07.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LINQ.Exercises
{
    public class Q20_Q30
    {
        public static void questions()
        {
            // OldesCustomer();
            //customersWithSameName();
            // customersWithSameAge();
            // customersWithSameNameAndAge();
            // customersLiveInChicago();
            // Top3Spint();
            // joinCystomersAndOrders();
            //avgTotalAmountByCustomer();
            // recentOrders();
            greaterTotalAmountByCustomer();

        }
      
        //Q:21. Retrieve the second oldest customer from each city.
        public static void OldesCustomer()
        {
            var customers = Repository.LoadCustomers();

            var secondOldestCustomers = customers.GroupBy(x => x.City).
                         Where(x => x.Count() > 2).
                         Select(x => x.OrderByDescending(c => c.Age).ElementAt(1));

            foreach (var customer in secondOldestCustomers)
            {
                Console.WriteLine(customer.ToString());
            }
        }

       
        //Q22 : Find customers who have the same name and live in different cities (case-insensitive).
        public static void customersWithSameName()
        {
            var customers = Repository.LoadCustomers();

            var customersWithNamesAndCity = customers.GroupBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
                .SelectMany(x => x.DistinctBy(z => z.City)).ToList();

            foreach (var customer in customersWithNamesAndCity)
            {
                Console.WriteLine(customer.ToString());
            }
        }


        //Q23 : Identify customers who share the same age and city but have different names.
        public static void customersWithSameAge()
        {
            var customers = Repository.LoadCustomers();

            var groupedCustomers = customers.GroupBy(x => new { Age = x.Age, City = x.City }).
                Where(x => x.Select(z => z.Name).Distinct().Count() > 1);
         
            foreach (var group in groupedCustomers)
            {
                Console.WriteLine($"Age: {group.Key.Age}, City: {group.Key.City}");
                foreach (var customer in group)
                {
                    Console.WriteLine($"   Name: {customer.Name}");
                }
            }
        }


        //Q24:12. Get a list of customers who have the same name and the same age but are from different cities.
        public static void customersWithSameNameAndAge()
        {
            var customers = Repository.LoadCustomers();

            var groupedCustomers = customers.GroupBy(x => new { Age = x.Age,    Name = x.Name }).
                Where(x => x.Select(z => z.City).Distinct().Count() > 1);

            foreach (var group in groupedCustomers)
            {
                Console.WriteLine($"Age: {group.Key.Age}, Name: {group.Key.Name}");
                foreach (var customer in group)
                {
                    Console.WriteLine($"   Name: {customer.City}");
                }
            }
        }


        //Q25:Retrieve a list of customers who are not older than 30 and live in Chicago.
        public static void customersLiveInChicago()
        {
            var customers = Repository.LoadCustomers();

            var groupedCustomers = customers.Where(x=>x.City.Equals("Chicago") || x.Age <30);

            foreach (var customer in groupedCustomers)
            {
                Console.WriteLine(customer.ToString());
            }
        }


        //Q26:1. Find the top 3 customers who spent the most in total on orders.
        public static void Top3Spint()
        {
            var Orders = Repository.LoadOrders();
            var customers = Repository.LoadCustomers();


            var result = Orders.GroupBy(x=>x.CustomerId)
            .Select(g => new
            {
                Customer = customers.First(c => c.Id == g.Key),
                TotalSpent = g.Sum(x => x.TotalAmount) 
            }).OrderByDescending(o =>o.TotalSpent).Take(3).ToList();    

            foreach (var customer in result)
            {
                Console.WriteLine($"{customer.Customer.ToString()} TotalAmount {customer.TotalSpent}  ");
            }
        }


        //Q27: Perform an inner join between customers and orders to retrieve the orders along with customer details.
        public static void joinCystomersAndOrders()
        {
            var Orders = Repository.LoadOrders();
            var customers = Repository.LoadCustomers();


            var result = Orders.Join(customers, o => o.CustomerId,
                                                c => c.Id,
                                                (o, c) => new
                                                {
                                                    customerName = c.Name,
                                                    OrderId = o.Id,
                                                    TotalAmount = o.TotalAmount,
                                                }).ToList();

            foreach (var customer in result)
            {
                Console.WriteLine($"customerName {customer.customerName}, OrderId {customer.OrderId} ,TotalAmount {customer.TotalAmount}  ");
            }
        }

        //Q28: Find the average order amount for each customer.
        public static void avgTotalAmountByCustomer()
        {
            var Orders = Repository.LoadOrders();
            var customers = Repository.LoadCustomers();


            var result = Orders.GroupBy(x => x.CustomerId).Select(a =>
            new {CustomerName= customers.First(c=>c.Id == a.Key).Name,
                Average=a.Average(c => c.TotalAmount) }).ToList();

            foreach (var customer in result)
            {
                Console.WriteLine($"customerName {customer.CustomerName}, Average {customer.Average}");
            }
        }

        //Q29: Get the most recent orders placed by each customer.
        public static void recentOrders()
        {
            var Orders = Repository.LoadOrders();
            var customers = Repository.LoadCustomers();


            var result = Orders.GroupBy(x => x.CustomerId)
                .Select(t => new
                {
                    CustomerName = customers.First(c => c.Id == t.Key).Name,
                    MostRecentOrder = t.OrderByDescending(o => o.OrderDate).FirstOrDefault().OrderDate
                }
                ).ToList();
               ;

            foreach (var customer in result)
            {
                Console.WriteLine($"customerName: {customer.CustomerName} ,OrderDate :{customer.MostRecentOrder} ");
            }
        }


        //Q30:Create a list of customers who have made orders with a total greater than a certain amount (e.g., $100).
        public static void greaterTotalAmountByCustomer()
        {
            var Orders = Repository.LoadOrders();
            var customers = Repository.LoadCustomers();


            var result = Orders.Where(x => x.TotalAmount >100).
                GroupBy(x => x.CustomerId).Select(a =>
            new {
                CustomerName = customers.First(c => c.Id == a.Key).Name,
                totalGreater = a.Sum(c => c.TotalAmount)
            }).ToList();

            foreach (var customer in result)
            {
                Console.WriteLine($"customerName {customer.CustomerName}, amount {customer.totalGreater}");
            }
        }


    }
}
