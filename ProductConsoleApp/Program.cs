using priceTracker;
using priceTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace ProductConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<MyDbcontext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            using var dbContext = new MyDbcontext(optionsBuilder.Options);

            var productsController = new ProductsController(dbContext);

            var entries = dbContext.Entrys.ToList();

            foreach (var entry in entries)
            {
                var result = productsController.CreateProductFromEntry(entry.Id);

                    Console.WriteLine($"Products created for entry ID {entry.Id}");
                
            }
        }
    }
}
