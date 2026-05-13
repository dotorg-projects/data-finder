// Copyright (c) 2025 .ORG Projects. All rights reserved.
// Licensed under the Microsoft Public License (Ms-PL).
// See LICENSE.txt in the project root for license information.
using DotOrgProjects.EntityPlatform.Finder.Sample.Entities;
using DotOrgProjects.EntityPlatform.Finder.Sample.Models;
using DotOrgProjects.EntityPlatform.Finder.Sample.Services;
using DotOrgProjects.EntityPlatform.Finder.Sample.Settings;

namespace DotOrgProjects.EntityPlatform.Finder.Sample
{
    public class Program
    {

        public ProductService Service { get; private set; }

        static void Main(string[] args)
        {

            Program program = new();

            program.Controller();

        }

        public Program()
        {
            ProgramSetting.Instantiate();
            Service = new();
        }

        public void Controller()
        {

            Console.WriteLine("--- Load Products ---");
            Service.Save(new ProductModel { Name = "Laptop", Price = 999.99m, Type = "Electronics" });
            Service.Save(new ProductModel { Name = "Phone", Price = 699.99m, Type = "Electronics" });
            Service.Save(new ProductModel { Name = "Tablet", Price = 499.99m, Type = "Electronics" });
            Service.Save(new ProductModel { Name = "T-Shirt", Price = 19.99m, Type = "Clothing" });
            Service.Save(new ProductModel { Name = "Jeans", Price = 49.99m, Type = "Clothing" });
            Service.Save(new ProductModel { Name = "Coffee", Price = 5.99m, Type = "Food" });
            Service.Save(new ProductModel { Name = "Bread", Price = 2.99m, Type = "Food" });

            Console.WriteLine("--- Show Products ---");
            foreach (ProductModel product in Service.FindAll())
            {
                Console.WriteLine($"Product: {product.Id} - {product.Name} - {product.Price} - {product.Type}");
            }

            Console.WriteLine("--- Show Products: Electronics ---");
            foreach (ProductModel product in Service.FindByType(new ProductModel { Type = "Electronics" }))
            {
                Console.WriteLine($"Product: {product.Id} - {product.Name} - {product.Price}");
            }

            Console.WriteLine("--- Show Products: Clothing ---");
            foreach (ProductModel product in Service.FindByType(new ProductModel { Type = "Clothing" }))
            {
                Console.WriteLine($"Product: {product.Id} - {product.Name} - {product.Price}");
            }

            Console.WriteLine("--- Show Products: Food ---");
            foreach (ProductModel product in Service.FindByType(new ProductModel { Type = "Food" }))
            {
                Console.WriteLine($"Product: {product.Id} - {product.Name} - {product.Price}");
            }

        }

    }
}
