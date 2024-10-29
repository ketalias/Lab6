using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace Lab6
{
    class Program
    {
        static void Main(string[] args)
        {
            Batteries.Init();

            using (var context = new ShopContext())
            {
                context.Database.EnsureCreated();

                if (!context.Products.Any())
                {
                    context.Products.AddRange(
                        new Product { Name = "Apples", Quantity = 50, Price = 0.5m, ExpirationDays = 7 },
                        new Product { Name = "Bread", Quantity = 30, Price = 1.2m, ExpirationDays = 3 },
                        new Product { Name = "Milk", Quantity = 20, Price = 1.5m, ExpirationDays = 5 },
                        new Product { Name = "Eggs", Quantity = 100, Price = 0.2m, ExpirationDays = 14 },
                        new Product { Name = "Butter", Quantity = 10, Price = 2.5m, ExpirationDays = 10 }
                    );
                    context.SaveChanges();
                    Console.WriteLine("Database seeded with sample products.");
                }
            }

            Console.WriteLine("Displaying all products:");
            DisplayProducts();

            Console.WriteLine("\nCalculating total value of all products:");
            CalculateTotalValue();

            Console.WriteLine("\nCounting expired products:");
            CountExpiredProducts();
        }

        public static void DisplayProducts()
        {
            using (var context = new ShopContext())
            {
                var products = context.Products.ToList();

                foreach (var product in products)
                {
                    Console.WriteLine($"Name: {product.Name}, Quantity: {product.Quantity}, Price: {product.Price}, " +
                                      $"ExpirationDays: {product.ExpirationDays}");
                }
            }
        }

        public static void CalculateTotalValue()
        {
            using (var context = new ShopContext())
            {
                var totalValue = context.Products.Sum(p => p.Quantity * (double)p.Price);
                Console.WriteLine($"Total value of all products in the store: {totalValue}");
            }
        }

        public static void CountExpiredProducts()
        {
            using (var context = new ShopContext())
            {
                var expiredProductsCount = context.Products
                    .Where(p => p.ExpirationDays <= 0)
                    .Sum(p => p.Quantity);

                Console.WriteLine($"Total expired products: {expiredProductsCount}");
            }
        }
    }
}
