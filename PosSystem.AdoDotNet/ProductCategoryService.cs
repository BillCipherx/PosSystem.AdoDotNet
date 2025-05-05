using Microsoft.Data.SqlClient;
using PosSystem.AdoDotNet.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PosSystem.AdoDotNet;

public class ProductCategoryService
{
    public readonly AdoDotNetService _adoDotNetService;

    public ProductCategoryService()
    {
        _adoDotNetService = new AdoDotNetService(AppSetting.ConnectionString);
    }

    // Create Product Category
    public void CreateProductCategory()
    {
        string category;
        while (true)
        {
            Console.WriteLine("Enter ProductCategory Name:  ");
            category = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(category))
            {
                Console.WriteLine("Category name cannot be empty. Please try again.");
            }
            else
            {
                break;
            }
        }

        string query = @"INSERT INTO [dbo].[ProductCategory]
                       ([ProductCategoryName])
                    VALUES
                       (@ProductCategoryName)";

        int result = _adoDotNetService.Execute(query, new SqlParameterModel
        {
            Name = "@ProductCategoryName",
            Value = category
        });

        /*var parameters = new List<SqlParameterModel>
        {
            new SqlParameterModel
            {
                Name = "@ProductCategoryName",
                Value = category
            }
        };

        int result = _adoDotNetService.Execute(query, parameters);*/

        Console.WriteLine(result == 1 ? "Creating Successful" : "Creating Failed");

    }

    // Read Product Categories
    public void ReadProductCategories()
    {
        string query = @"SELECT [ProductCategoryId]
                        ,[ProductCategoryName]
                        FROM [dbo].[ProductCategory]";

        var dt = _adoDotNetService.Query(query);

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                Console.WriteLine("ProductCategoryId : " + dr["ProductCategoryId"]);
                Console.WriteLine("ProductCategoryName : " + dr["ProductCategoryName"]);
            }
        }
        else
        {
            Console.WriteLine("! No data found !");
        }

    }

    // Update Product Categoty
    public void UpdateProductCategory()
    {
        int id;
        while(true)
        {
            Console.Write("Enter Product Category ID to Update: ");
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid ID input.");
            }
            else
            {
                break;
            }
        }

        Console.Write("Enter New Product Category Name: ");
        string category = Console.ReadLine();

        string query = @"UPDATE [dbo].[ProductCategory]
                       SET [ProductCategoryName] = @ProductCategoryName
                     WHERE ProductCategoryId = @ProductCategoryId";

        int result = _adoDotNetService.Execute(query, new SqlParameterModel
        {
            Name = "@ProductCategoryId",
            Value = id
        }, new SqlParameterModel
        {
            Name = "@ProductCategoryName",
            Value = category
        });

        Console.WriteLine(result == 1 ? "Updating Successful" : "Updating Failed");
    }

    // Delete Product Category
    public void DeleteProductCategory()
    {
        int id;
        while(true)
        {
            Console.Write("Enter Product Category ID to Delete: ");
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid ID input.");
            }
            else
            {
                break;
            }
        }

        string query = @"DELETE FROM [dbo].[ProductCategory]
                            WHERE ProductCategoryId = @Id";

        int result =_adoDotNetService.Execute(query, new SqlParameterModel
        {
            Name = "@Id",
            Value = id
        });

        Console.WriteLine(result > 0 ? "Deleting Successfully" : "Deleting Failed.");
    }
}
