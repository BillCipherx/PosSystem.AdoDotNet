using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosSystem.AdoDotNet
{
    public class Product
    {
        private readonly string _connectionString = @"Data Source=.;Initial Catalog=PosSystem.AdoDotNet;User ID=sa;Password=sasa@123;Trusted_Connection=True;";

        // Create Product
        public void CreateProduct()
        {
            Console.Write("Enter Product Name: ");
            string name = Console.ReadLine();
            decimal price;
            while(true)
            {
                Console.Write("Enter Product Price: ");
                string input = (Console.ReadLine());
                if(decimal.TryParse(input, out price) )
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid price. Please enter a number value.");
                }
            }
            Console.Write("Enter Product ID: ");
            int categoryId = int.Parse(Console.ReadLine());

            SqlConnection connection = new SqlConnection(_connectionString);

            connection.Open();

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
            SqlConnection connection = new SqlConnection(_connectionString);
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

            SqlConnection connection = new SqlConnection(_connectionString);

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
            Console.Write("Enter Product ID to Delete: ");
            int id = int.Parse(Console.ReadLine());

            SqlConnection connection = new SqlConnection(_connectionString);

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
