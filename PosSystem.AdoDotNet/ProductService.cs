using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using PosSystem.AdoDotNet.Shared;

namespace PosSystem.AdoDotNet;

public class ProductService
{
    public readonly AdoDotNetService _adoDotNetService;

    public ProductService()
    {
        _adoDotNetService = new AdoDotNetService(AppSetting.ConnectionString);
    }

    // Create Product
    public void CreateProduct()
    {
        string name;
        while (true)
        {
            Console.Write("Enter Product Name: ");
            name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Category name cannot be empty. Please try again.");
            }
            else
            {
                break;
            }
        }

        decimal price;
        while(true)
        {
            Console.Write("Enter Product Price: ");
            if(decimal.TryParse((Console.ReadLine()), out price) && price > 0 )
            {
                break;
            }   
            else
            {
                Console.WriteLine("Invalid price. Please enter a number value.");
            }
        }
        int categoryId;
        while(true)
        {
            Console.Write("Enter Product Category ID: ");
            if (int.TryParse((Console.ReadLine()), out categoryId))
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid product categoryid. Please try again.");
            }
        }

        string categoryQuery = "SELECT COUNT(*) FROM ProductCategory WHERE ProductCategoryId = @CategoryId";

        int categoryExists = _adoDotNetService.Execute(categoryQuery, new SqlParameterModel
        {
            Name = "@CategoryId",
            Value = categoryId
        });

        if (categoryExists == 0)
        {
            Console.WriteLine("Product Category ID does not exist. Please add the category first or choose a valid one.");
            
            return;
        }

        string query = @"INSERT INTO [dbo].[Product]
                       ([ProductName]
                       ,[Price]
                       ,[ProductCategoryId])
                 VALUES
                       (@ProductName
                       ,@Price
                       ,@ProductCategoryId)";

        int result = _adoDotNetService.Execute(query, new SqlParameterModel
        {
            Name = "@ProductName",
            Value = name
        },new SqlParameterModel
        {
            Name = "@Price",
            Value = price
        },new SqlParameterModel
        {
            Name = "@ProductCategoryId",
            Value = categoryId
        });

        Console.WriteLine(result == 1 ? "Creating Successful" : "Creating Failed");
    }

    // Read Products
    public void ReadProducts()
    {
        string query = @"SELECT [ProductId]
                      ,[ProductName]
                      ,[Price]
                      ,[ProductCategoryId]
                  FROM [dbo].[Product]";

        var dt = _adoDotNetService.Query(query);

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                Console.WriteLine($"ID: {dr["ProductId"]}, Name: {dr["ProductName"]}, Price: {dr["Price"]}, CategoryId: {dr["ProductCategoryId"]}");
            }
        }
    }

    // Update Product
    public void UpdateProduct()
    {
        int id;
        while (true)
        {
            Console.Write("Enter Product ID to Update: ");
            string input = (Console.ReadLine());
            if(int.TryParse(input, out id))
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid ID. Please enter a number value.");
            }
        }

        Console.Write("Enter New Product Name: ");
        string name = Console.ReadLine();
        decimal price;
        while (true)
        {
            Console.Write("Enter New Product Price: ");
            string input = (Console.ReadLine());
            if(decimal.TryParse(input, out price))
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid price. Please enter a number value.");
            }
        }

        int categoryId;
        while (true)
        {
            Console.Write("Enter New Product Category ID: ");
            string input = (Console.ReadLine());
            if (int.TryParse(input, out categoryId))
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid ID. Please enter a number value.");
            }
        }

        string query = @"UPDATE [dbo].[Product]
                       SET [ProductName] = @ProductName
                          ,[Price] = @Price
                          ,[ProductCategoryId] = @ProductCategoryId
                     WHERE ProductId = @ProductId";

        int result = _adoDotNetService.Execute(query, new SqlParameterModel
        {
            Name = "@ProductId",
            Value = id
        },new SqlParameterModel
        {
            Name = "@ProductName",
            Value = name
        }, new SqlParameterModel
        {
            Name = "@Price",
            Value = price
        }, new SqlParameterModel
        {
            Name = "@ProductCategoryId",
            Value = categoryId
        });

        Console.WriteLine(result == 1 ? "Updating Successful" : "Updating Failed");
    }

    // Delete Product
    public void DeleteProduct()
    {
        int id;
        while (true)
        {
            Console.Write("Enter Product ID to Delete: ");
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid ID input.");
            }
            else
            {
                break;
            }
        }

        string query = @"DELETE FROM [dbo].[Product]
                        WHERE ProductId = @Id";

        int result = _adoDotNetService.Execute(query, new SqlParameterModel
        {
            Name = "@Id",
            Value = id
        });
        Console.WriteLine(result > 0 ? "Deleting Successfully" : "Deleting Failed.");
    }

}
