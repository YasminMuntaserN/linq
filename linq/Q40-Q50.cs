using LINQ.Exercises.data;
using LINQ.Exercises.data.LINQTut07.Shared;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LINQ.Exercises
{
    public class Q40_Q50
    {
        public static void questions()
        {
            // HighestSpendingCustomer();
            // SameOrder();
            // CustomersWithNoOrders();
            // CustomerOrdersInPastyear();
            // LongestTimeBetweenTwoOrders();
            //  AveragePriceOfTheProducts();
            // CustomerswithMultipleOrders();
            // CitiesWitCustomer();
            // TotalRevenueForEachProduct();
            CustomersWithTheLongestOrderHistory();

        }
        //Q41:Find the highest spending customer for each city, showing the customer's name, city, and total amount spent.
        public static void HighestSpendingCustomer()
        {
            var customers = Repository.LoadCustomers();
            var orders = Repository.LoadOrders();

            var result = customers.GroupBy(c => c.City).
            Select(customer => new
            {
                city = customer.Key,
                customer = customer.Select(x => new
                {
                    customerName = x.Name,
                    totalAmount = orders.Where(o => (x.Id) == o.CustomerId).Sum(o => o.TotalAmount),
                }).OrderByDescending(x => x.totalAmount).FirstOrDefault()
            }
            );

            foreach (var re in result)
            {
                Console.WriteLine($" CustomerName = {re.customer.customerName}, city = {re.city} ,spent Amount ={re.customer.totalAmount} ");
            }
        }

        //Q42:Customers Who Ordered the Same Product Multiple Times******************************
        public static void SameOrder()
        {
            var customers = Repository.LoadCustomers();
            var orders = Repository.LoadOrders();

            // var result = customers.Where(x=>orders.Any(o=>o.CustomerId == x.Id)).
            var result = orders.GroupBy(o => o.CustomerId).
             Select(group => new
             {
                 CustomerId = group.Key,
                 Products = group.Select(x => new
                 {
                     ProductsCount = x.Products.Select(x => x.Id).Count(),
                 }).Count()
             });


            foreach (var re in result)
            {
                Console.WriteLine($" CustomerId = {re.CustomerId}, ProductsCount = {re.Products} ");
            }
        }


        //Q43:Customers with No Orders in the Last 6 Months
        public static void CustomersWithNoOrders()
        {
            var sixMonthsAgo = DateTime.Now.AddMonths(-6);
            var customers = Repository.LoadCustomers();
            var orders = Repository.LoadOrders();

            var result = orders.GroupBy(o => o.CustomerId).
             Select(group => new
             {
                 CustomerId = group.Key,
                 OrderDate = group.OrderByDescending(x => x.OrderDate.Month).First().OrderDate,
             }).Where(x => x.OrderDate >= sixMonthsAgo);


            foreach (var re in result)
            {
                Console.WriteLine($" CustomerId = {re.CustomerId}, ProductsCount = {re.OrderDate} ");
            }
        }


        //Q44:Find customers who haven’t placed an order in the past year.
        //Display their name and the date of their last order.
        public static void CustomerOrdersInPastyear()
        {
            var customers = Repository.LoadCustomers();
            var orders = Repository.LoadOrders();

            var result = orders.GroupBy(o => o.CustomerId).
             Select(group => new
             {
                 CustomerId = group.Key,
                 orderYear = group.OrderByDescending(p => p.OrderDate.Year).First().OrderDate,
             }).Where(o => o.orderYear.Year < DateTime.Now.Year);


            foreach (var re in result)
            {
                Console.WriteLine($" CustomerId = {re.CustomerId} orderYear ={re.orderYear} ");
            }
        }


        //Q45:  Identify customers who have placed more than 3 orders and display their name and total number of orders.
        public static void LongestTimeBetweenTwoOrders()
        {
            var customers = Repository.LoadCustomers();
            var orders = Repository.LoadOrders();

            var result = orders.GroupBy(o => o.CustomerId).
             Select(group => new
             {
                 CustomerId = group.Key,
                 totalNumersOfOrders = group.Count()
             }).Where(o => o.totalNumersOfOrders >= 3).
             Join(customers,
             order => order.CustomerId,
             customer => customer.Id,
             (order, customer) => new { CustomerName = customer.Name, order.totalNumersOfOrders }
             );


            foreach (var re in result)
            {
                Console.WriteLine($" CustomerName = {re.CustomerName} total number of orders ={re.totalNumersOfOrders} ");
            }
        }


        //Q46:For each order, calculate the average price of the products in that order. Display the order ID and the average product price.
        public static void AveragePriceOfTheProducts()
        {
            var customers = Repository.LoadCustomers();
            var orders = Repository.LoadOrders();

            var result = orders.
             Select(group => new
             {
                 orderID = group.Id,
                 average = group.Products.Average(p => p.Price)
             });


            foreach (var re in result)
            {
                Console.WriteLine($" CustomerName = {re.orderID} average ={re.average} ");
            }
        }

        
        //Q47:Find customers who have placed multiple orders on the same day.
        public static void CustomerswithMultipleOrders()
        {
            var customers = Repository.LoadCustomers();
            var orders = Repository.LoadOrders();

            var result = orders.GroupBy(o => o.CustomerId).
             Select(group => new
             {
                 CustomerId = group.Key,
                 orderDate = group.GroupBy(p => p.OrderDate)
                 .Where(g => g.Count() > 1) 
                .Select(g => g.Key)
             }).Where(x => x.orderDate.Any());


            foreach (var re in result)
            {
                Console.WriteLine($"CustomerId = {re.CustomerId}, Duplicate Order Dates: {string.Join(", ", re.orderDate)}");
            }
        }



        //Q48 :Cities with All Customers Having Placed Orders
        public static void CitiesWitCustomer()
        {
            var customers = Repository.LoadCustomers();
            var orders = Repository.LoadOrders();

            var result = customers.GroupBy(c => c.City).
                Where(group => group.All(A => orders.Any(o => o.Id == A.Id))).
                Select(group => group.Key);

            Console.WriteLine($" Cities with All Customers Having Placed Orders ");

            foreach (var re in result)
            {
                Console.WriteLine($" City :  {re} ");

            }
        }

      
        //Q49:Group all orders by product and calculate the total revenue generated for each product. Display the product name and the total revenue.
        public static void TotalRevenueForEachProduct()
        {
            var orders = Repository.LoadOrders();

            var result = orders.SelectMany(x=>x.Products).GroupBy(c => c.Name).
            Select(group => new
            {
                ProductName = group.Key,
                TotalRevenue = group.Sum(x => x.Price),
            }
            );

            Console.WriteLine("All orders grouped by product, showing total revenue for each:");

            foreach (var item in result)
            {
                Console.WriteLine($"Product Name: {item.ProductName}, Total Revenue: {item.TotalRevenue}");
            }
        }


        // Q50 :Find Customers with the Longest Order History
        // Identify customers who have been placing orders for the longest duration(based on the time difference between their first and last orders). Display the customer name and the duration in days.
        public static void CustomersWithTheLongestOrderHistory()
        {
            var customers = Repository.LoadCustomers();
            var orders = Repository.LoadOrders();

            var result = orders.GroupBy(c => c.CustomerId).Select(group => new
            {
                customerName = customers.First(x => x.Id == group.Key).Name,
                firstOrderDate = group.Min(o => o.OrderDate),
                lastOrderDate = group.Max(o => o.OrderDate)
            }).Select(x => new
            {
                x.customerName,
                duration = (x.lastOrderDate - x.firstOrderDate).Days
            }
            ).OrderByDescending(x => x.duration).ToList();

            Console.WriteLine($"Customers with the Longest Order History");

            foreach (var re in result)
            {
                Console.WriteLine($"name {re.customerName} and the duration in days{re.duration}");

            }
        }
    }
}
