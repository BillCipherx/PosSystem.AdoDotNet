using PosSystem.AdoDotNet;
using System;
using System.Data;
using System.Data.SqlClient;

namespace POSConsoleApp
{
    class Program
    {
        static string connectionString = @"Data Source=.;Initial Catalog=PosSystem.AdoDotNet;User ID=sa;Password=sasa@123;Trusted_Connection=True;";
        SqlConnection connection = new SqlConnection(connectionString);

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n=== POS System ===");
                Console.WriteLine("1. Manage Product Category");
                Console.WriteLine("2. Manage Product");
                Console.WriteLine("3. Sale Products");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");
                string mainChoice = Console.ReadLine();

                switch (mainChoice)
                {
                    case "1":
                        ManageProductCategory();
                        break;
                    case "2":
                        ManageProduct();
                        break;
                    case "3":
                        Sale();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        static void ManageProductCategory()
        {
            ProductCategory productCategory = new ProductCategory();
            while (true)
            {
                Console.WriteLine("\n--- Product Category CRUD ---");
                Console.WriteLine("1. Create");
                Console.WriteLine("2. Read");
                Console.WriteLine("3. Update");
                Console.WriteLine("4. Delete");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Choose an action: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        productCategory.CreateProductCategory();
                        break;
                    case "2":
                        productCategory.ReadProductCategories();
                        break;
                    case "3":
                        productCategory.UpdateProductCategory();
                        break;
                    case "4":
                        productCategory.DeleteProductCategory();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        static void ManageProduct()
        {
            Product product = new Product();
            while (true)
            {
                Console.WriteLine("\n--- Product CRUD ---");
                Console.WriteLine("1. Create");
                Console.WriteLine("2. Read");
                Console.WriteLine("3. Update");
                Console.WriteLine("4. Delete");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Choose an action: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        product.CreateProduct();
                        break;
                    case "2":
                        product.ReadProducts();
                        break;
                    case "3":
                        product.UpdateProduct();
                        break;
                    case "4":
                        product.DeleteProduct();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        static void Sale()
        {
            Sale sale = new Sale();
            while(true)
            {
                Console.WriteLine("\n--- Product Sale ---");
                Console.WriteLine("1. Create");
                Console.WriteLine("2. Read");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Choose an action: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        sale.CreateSale();
                        break;
                    case "2":
                        sale.ReadSales();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }

            }
        }
    }
}
