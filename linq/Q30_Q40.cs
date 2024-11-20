using LINQ.Exercises.data;
using LINQ.Exercises.data.LINQTut07.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace LINQ.Exercises
{
    public class Q30_Q40
    {
        public static void questions()
        {
            //mostOrders();
            //TopProducts();
            //CustomersNeverOrder();
            //TopSpending();
            //AveragePerCity();
            // DuplicateProducts();
            // CustomersWhoSpentMoreThan2000();
            // CitiesWithHighestOrders();
            // MostExpensiveProduct();
            GroupOrdersByMonth();
        }


        //q31:Find the customer who has placed the most orders.
        public static void mostOrders()
        {
            var customers = Repository.LoadCustomers();
            var orders = Repository.LoadOrders();

            var secondOldestCustomers = orders.GroupBy(x => x.CustomerId)
                          .OrderByDescending(x => x.Count()).
                          Select(x => new { Customer = customers.First(c => c.Id == x.Key) });

            foreach (var customer in secondOldestCustomers)
            {
                Console.WriteLine(customer.ToString());
            }
        }

       
        //q32: Retrieve the top 5 products ordered the most across all customers.
        public static void TopProducts()
        {
            var orders = Repository.LoadOrders();

            var result = orders
                    .SelectMany(o => o.Products)
                    .GroupBy(p => p.Id)         
                    .OrderByDescending(g => g.Count()) 
                    .Take(5)                   
                    .Select(g => new
                    {
                        ProductId = g.Key,
                        TotalOrdered = g.Count()
                    })
                    .ToList();

            // Display the results
            foreach (var product in result)
            {
                Console.WriteLine($"Product ID: {product.ProductId}, Total Ordered: {product.TotalOrdered}");
            }
        }

       
        //q33: Retrieve a list of customers who have never placed an order.
        public static void CustomersNeverOrder()
        {
            var customers = Repository.LoadCustomers();
            var orders = Repository.LoadOrders();

            var customersWithoutOrders = customers
                .Where(customer => !orders.Any(order => order.CustomerId == customer.Id));

            foreach (var customer in customersWithoutOrders)
            {
                Console.WriteLine($"Customer ID = {customer.Id}, Name = {customer.Name} has never placed an order.");
            }
        }
       
        
        //q34: Get Top 3 Customers by Spending
        public static void TopSpending()
        {
            var customers = Repository.LoadCustomers();
            var orders = Repository.LoadOrders();

            var result = customers.Select(x => new { Name = x.Name, totalSpent = orders.Where(o => o.CustomerId == x.Id).Sum(x => x.TotalAmount) })
                .OrderByDescending(x => x.totalSpent).Take(3);

            foreach (var customer in result)
            {
                Console.WriteLine($" Name = {customer.Name} ,totalSpent={customer.totalSpent} ");
            }

            //another solution :
            var topSpendingCustomers = Repository.LoadOrders()
                .GroupBy(order => order.CustomerId)
                .Select(group => new
                {
                    CustomerName = Repository.LoadCustomers()
                        .FirstOrDefault(c => c.Id == group.Key)?.Name,
                    TotalSpent = group.Sum(order => order.TotalAmount)
                })
                .OrderByDescending(entry => entry.TotalSpent)
                .Take(3);

            foreach (var customer in topSpendingCustomers)
            {
                Console.WriteLine($"{customer.CustomerName} spent a total of ${customer.TotalSpent}.");
            }

        }
       
        
        //q35 :Calculate Average Order Amount Per City
        public static void AveragePerCity()
        {
            var customers = Repository.LoadCustomers();
            var orders = Repository.LoadOrders();

            var result = customers.GroupBy(c => c.City).
                Select(cit => new
                {
                    city = cit.Key,
                    Average = orders.Where(o => cit.Any(c => o.CustomerId == c.Id)).Average(X => X.TotalAmount)
                } );

            foreach (var customer in result)
            {
                Console.WriteLine($" city = {customer.city} ,Average Order Amount ={customer.Average} ");
            }
        }

       
        
        //Q36:Find Orders with Duplicate Products
        public static void DuplicateProducts()
        {
            var orders = Repository.LoadOrders();

            var result = orders.Select(order =>
            new
            {
                order = order.Id,
                product = order.Products.GroupBy(p => p.Id).Where(group => group.Count() >1)
                .Select(product => new
                {
                    ProductId = product.Key,
                    productCount = product.Select(p => new { Name = p.Name }).Count(),  
                }

                    )
            });
            foreach (var r in result)
            {
                Console.WriteLine($"Order ID: {r.order}");
                foreach (var p in r.product)
                {
                    Console.WriteLine($" Duplicate Products: Product ID {p.ProductId} Count: {p.productCount})");
                }
            }
        }

        
        
        // Q37 : Write a LINQ query to find customers who have spent more than $2000 in total across all their orders.
        public static void CustomersWhoSpentMoreThan2000()
        {
            var customers = Repository.LoadCustomers();
            var orders = Repository.LoadOrders();

            var result = customers.GroupBy(c => c.Id).
                Select(x => new
                {
                    cusromerId = x.Key,
                    spent = orders.Where(o => o.CustomerId == x.Key).Sum(X => X.TotalAmount)
                }).Where(s => s.spent > 2000);

            foreach (var customer in result)
            {
                Console.WriteLine($" cusromerId = {customer.cusromerId} ,spent Amount ={customer.spent} ");
            }
        }

        
        
        //Q38: identify which cities have the highest number of orders placed, and display the city name along with the order count.
        public static void CitiesWithHighestOrders()
        {
            var customers = Repository.LoadCustomers();
            var orders = Repository.LoadOrders();

            var result = customers
                .GroupBy(c => c.City) 
                .Select(group => new
                {
                    City = group.Key,
                    OrdersCount = orders.Count(o => group.Any(c => c.Id == o.CustomerId)) 
                })
                .OrderByDescending(city => city.OrdersCount)
                .FirstOrDefault(); 

                Console.WriteLine($"City: {result.City}, Orders Count: {result.OrdersCount}");
        }


       
        //Q39:Create a LINQ query to determine the most expensive product each customer has ever ordered. Display the customer's name and the product details.
        public static void MostExpensiveProduct()
        {
            var customers = Repository.LoadCustomers();
            var orders = Repository.LoadOrders();

            var result = orders
                .GroupBy(c => c.CustomerId)
                .Select(group => new
                {
                    CustomerId = group.Key,
                    expensiveProduct = group.SelectMany(p => p.Products).OrderByDescending(p=>p.Price).First(),
                });


            foreach (var customer in result)
            {
                Console.WriteLine($" cusromerId = {customer.CustomerId} ,expensiveProduct ={customer.expensiveProduct.Name} ");
            }
        }


       
        //Q40:2. Total Sales by Month
        // Group orders by the month they were placed and calculate the total sales for each month.
        // Display the month (as a name or number) and the total sales amount.
        public static void GroupOrdersByMonth()
        {
            var orders = Repository.LoadOrders();

            var result = orders
                .GroupBy(c => c.OrderDate.Month)
                .Select(group => new
                {
                    Month = group.Key,
                    totalSales = group.Sum(sales=> sales.TotalAmount),
                });


            foreach (var re in result)
            {
                Console.WriteLine($" Month = {re.Month} ,expensiveProduct ={re.totalSales} ");
            }
        }
    }
}
