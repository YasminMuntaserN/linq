using LINQ.Exercises.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQ.Exercises.data
{
    namespace LINQTut07.Shared
    {
        public static class Repository
        {
            public static IEnumerable<Customer> LoadCustomers()
            {
                return new List<Customer> {
                new Customer { Id = 1, Name = "John Doe", City = "New York", Age = 35 },
                new Customer { Id = 2, Name = "Jane Smith", City = "Los Angeles", Age = 28 },
                new Customer { Id = 3, Name = "Michael Johnson", City = "Chicago", Age = 42 },
                new Customer { Id = 4, Name = "Emily Davis", City = "Houston", Age = 30 },
                new Customer { Id = 5, Name = "David Martinez", City = "Boston", Age = 37 },
                new Customer { Id = 6, Name = "Sarah Wilson", City = "Philadelphia", Age = 29 },
                new Customer { Id = 7, Name = "James Brown", City = "San Antonio", Age = 45 },
                new Customer { Id = 8, Name = "Patricia Garcia", City = "New York", Age = 35 },
                new Customer { Id = 9, Name = "Robert Lee", City = "San Diego", Age = 38 },
                new Customer { Id = 10, Name = "Linda Harris", City = "San Jose", Age = 41 },
                new Customer { Id = 11, Name = "William Clark", City = "Boston", Age = 33 },
                new Customer { Id = 12, Name = "Elizabeth Lewis", City = "Jacksonville", Age = 39 },
                new Customer { Id = 13, Name = "Charles Walker", City = "New York", Age = 50 },
                new Customer { Id = 14, Name = "Jessica Young", City = "Chicago", Age = 27 },
                new Customer { Id = 15, Name = "Daniel Hall", City = "New York", Age = 36 },
                new Customer { Id = 16, Name = "Susan Allen", City = "Indianapolis", Age = 48 },
                new Customer { Id = 17, Name = "Barbara Scott", City = "New York", Age = 40 },
                new Customer { Id = 18, Name = "Nancy Wright", City = "Denver", Age = 26 },
                new Customer { Id = 19, Name = "George Lopez", City = "Boston", Age = 44 },
                new Customer { Id = 20, Name = "Barbara Scott", City = "Boston", Age = 40 }
            };
            }

            public static IEnumerable<Order> LoadOrders()
            {
                return new List<Order>
    {
        new Order
        {
            Id = 1,
            CustomerId = 1,
            OrderDate = new DateTime(2023, 1, 15),
            Products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 1200.50m },
                new Product { Id = 2, Name = "Mouse", Price = 25.00m }
            },
            TotalAmount = 1225.50m
        },
        new Order
        {
            Id = 2,
            CustomerId = 2,
            OrderDate = new DateTime(2022, 2, 10),
            Products = new List<Product>
            {
                new Product { Id = 3, Name = "Smartphone", Price = 800.00m }
            },
            TotalAmount = 800.00m
        },
        new Order
        {
            Id = 3,
            CustomerId = 3,
            OrderDate = new DateTime(2024, 3, 5),
            Products = new List<Product>
            {
                new Product { Id = 4, Name = "Keyboard", Price = 50.00m },
                new Product { Id = 5, Name = "Monitor", Price = 300.00m }
            },
            TotalAmount = 350.00m
        },
        new Order
        {
            Id = 4,
            CustomerId = 4,
            OrderDate = new DateTime(2024, 1, 25),
            Products = new List<Product>
            {
                new Product { Id = 6, Name = "Tablet", Price = 600.00m }
            },
            TotalAmount = 600.00m
        },
        new Order
        {
            Id = 5,
            CustomerId = 5,
            OrderDate = new DateTime(2024, 4, 18),
            Products = new List<Product>
            {
                new Product { Id = 8, Name = "Printer", Price = 200.00m },
                new Product { Id = 7, Name = "Headphones", Price = 150.00m },
                new Product { Id = 8, Name = "Printer", Price = 200.00m },
                new Product { Id = 6, Name = "Tablet", Price = 600.00m }
            },
            TotalAmount = 150.00m
        },
        new Order
        {
            Id = 6,
            CustomerId = 6,
            OrderDate = new DateTime(2024, 5, 12),
            Products = new List<Product>
            {
                new Product { Id = 8, Name = "Printer", Price = 200.00m },
                new Product { Id = 7, Name = "Headphones", Price = 150.00m },
                new Product { Id = 8, Name = "Printer", Price = 200.00m }

            },
            TotalAmount = 200.00m
        },
        new Order
        {
            Id = 7,
            CustomerId = 7,
            OrderDate = new DateTime(2024, 6, 1),
            Products = new List<Product>
            {
                new Product { Id = 9, Name = "Router", Price = 120.00m }
            },
            TotalAmount = 120.00m
        },
        new Order
        {
            Id = 8,
            CustomerId = 8,
            OrderDate = new DateTime(2024, 7, 20),
            Products = new List<Product>
            {
                new Product { Id = 10, Name = "Webcam", Price = 80.00m }
            },
            TotalAmount = 80.00m
        },
        new Order
        {
            Id = 9,
            CustomerId = 9,
            OrderDate = new DateTime(2024, 8, 14),
            Products = new List<Product>
            {
                new Product { Id = 11, Name = "External Hard Drive", Price = 150.00m }
            },
            TotalAmount = 150.00m
        },
        new Order
        {
            Id = 10,
            CustomerId = 10,
            OrderDate = new DateTime(2024, 9, 8),
            Products = new List<Product>
            {
                new Product { Id = 12, Name = "Smartwatch", Price = 300.00m }
            },
            TotalAmount = 300.00m
        },
        new Order
        {
            Id = 11,
            CustomerId = 11,
            OrderDate = new DateTime(2024, 9, 15),
            Products = new List<Product>
            {
                new Product { Id = 13, Name = "Gaming Console", Price = 400.00m }
            },
            TotalAmount = 400.00m
        },
        new Order
        {
            Id = 12,
            CustomerId = 12,
            OrderDate = new DateTime(2024, 10, 3),
            Products = new List<Product>
            {
                new Product { Id = 14, Name = "VR Headset", Price = 600.00m }
            },
            TotalAmount = 600.00m
        },
        new Order
        {
            Id = 13,
            CustomerId = 13,
            OrderDate = new DateTime(2024, 10, 20),
            Products = new List<Product>
            {
                new Product { Id = 15, Name = "Camera", Price = 1200.00m }
            },
            TotalAmount = 1200.00m
        },
        new Order
        {
            Id = 14,
            CustomerId = 14,
            OrderDate = new DateTime(2024, 11, 1),
            Products = new List<Product>
            {
                new Product { Id = 16, Name = "Microwave", Price = 100.00m }
            },
            TotalAmount = 100.00m
        },
        new Order
        {
            Id = 15,
            CustomerId = 15,
            OrderDate = new DateTime(2024, 11, 5),
            Products = new List<Product>
            {
                new Product { Id = 17, Name = "Dishwasher", Price = 800.00m }
            },
            TotalAmount = 800.00m
        },
        new Order
        {
            Id = 16,
            CustomerId = 16,
            OrderDate = new DateTime(2024, 11, 10),
            Products = new List<Product>
            {
                new Product { Id = 18, Name = "Refrigerator", Price = 1500.00m }
            },
            TotalAmount = 1500.00m
        },
        new Order
        {
            Id = 17,
            CustomerId = 17,
            OrderDate = new DateTime(2024, 11, 15),
            Products = new List<Product>
            {
                new Product { Id = 19, Name = "TV", Price = 2000.00m }
            },
            TotalAmount = 2000.00m
        },
        new Order
        {
            Id = 18,
            CustomerId = 18,
            OrderDate = new DateTime(2024, 11, 20),
            Products = new List<Product>
            {
                new Product { Id = 20, Name = "Gaming Laptop", Price = 2500.00m }
            },
            TotalAmount = 2500.00m
        },
        new Order
        {
            Id = 19,
            CustomerId = 19,
            OrderDate = new DateTime(2024, 11, 25),
            Products = new List<Product>
            {
                new Product { Id = 21, Name = "Electric Scooter", Price = 700.00m }
            },
            TotalAmount = 700.00m
        },
        new Order
        {
            Id = 20,
            CustomerId = 20,
            OrderDate = new DateTime(2024, 12, 7),
            Products = new List<Product>
            {
                new Product { Id = 22, Name = "Electric Bicycle", Price = 1200.00m }
            },
            TotalAmount = 1200.00m
        },
                new Order
        {
            Id = 21,
            CustomerId = 20,
            OrderDate = new DateTime(2024, 12, 1),
            Products = new List<Product>
            {
                new Product { Id = 22, Name = "Electric Bicycle", Price = 1200.00m }
            },
            TotalAmount = 1200.00m
        },
                        new Order
        {
            Id = 22,
            CustomerId = 20,
            OrderDate = new DateTime(2024, 12, 5),
            Products = new List<Product>
            {
                new Product { Id = 22, Name = "Electric Bicycle", Price = 1200.00m }
            },
            TotalAmount = 1200.00m
        },
    };
            }
        }
    }
}
