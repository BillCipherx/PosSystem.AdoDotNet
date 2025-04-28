using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace PosSystem.AdoDotNet
{
    public class ProductService
    {
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
            
            SqlConnection connection = new SqlConnection(AppSetting.ConnectionString);

            connection.Open();

            string categoryQuery = "SELECT COUNT(*) FROM ProductCategory WHERE ProductCategoryId = @CategoryId";

            SqlCommand categoryCmd = new SqlCommand(categoryQuery, connection);
            categoryCmd.Parameters.AddWithValue("@CategoryId", categoryId);
            int categoryExists = (int)categoryCmd.ExecuteScalar();

            if (categoryExists == 0)
            {
                Console.WriteLine("Product Category ID does not exist. Please add the category first or choose a valid one.");
                connection.Close();
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

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@ProductName", name);
            cmd.Parameters.AddWithValue("@Price", price);
            cmd.Parameters.AddWithValue("@ProductCategoryId", categoryId);
            int result = cmd.ExecuteNonQuery();

            connection.Close(); 

            Console.WriteLine(result == 1 ? "Creating Successful" : "Creating Failed");
        }

        // Read Products
        public void ReadProducts()
        {
            SqlConnection connection = new SqlConnection(AppSetting.ConnectionString);
            connection.Open();

            string query = @"SELECT [ProductId]
                          ,[ProductName]
                          ,[Price]
                          ,[ProductCategoryId]
                      FROM [dbo].[Product]";

            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            connection.Close();

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

            Console.Write("Enter New Product Category ID: ");
            int categoryId = int.Parse(Console.ReadLine());

            SqlConnection connection = new SqlConnection(AppSetting.ConnectionString);

            connection.Open();

            string query = @"UPDATE [dbo].[Product]
                           SET [ProductName] = @ProductName
                              ,[Price] = @Price
                              ,[ProductCategoryId] = @ProductCategoryId
                         WHERE ProductId = @ProductId";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@ProductId", id);
            cmd.Parameters.AddWithValue("@ProductName", name);
            cmd.Parameters.AddWithValue("@Price", price);
            cmd.Parameters.AddWithValue("@ProductCategoryId", categoryId);
            int result = cmd.ExecuteNonQuery();

            Console.WriteLine(result == 1 ? "Updating Successful" : "Updating Failed");

            connection.Close();
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

            SqlConnection connection = new SqlConnection(AppSetting.ConnectionString);

            connection.Open();

            string query = @"DELETE FROM [dbo].[Product]
                            WHERE ProductId = @Id";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", id);
            int result = cmd.ExecuteNonQuery();
            Console.WriteLine(result > 0 ? "Deleting Successfully" : "Deleting Failed.");

            connection.Close();
        }
    }
}
