using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosSystem.AdoDotNet
{
    public class ProductCategory
    {
        private readonly string _connectionString = @"Data Source=.;Initial Catalog=PosSystem.AdoDotNet;User ID=sa;Password=sasa@123;Trusted_Connection=True;";
        
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

            SqlConnection connection = new SqlConnection(_connectionString);

            connection.Open();

            string query = @"INSERT INTO [dbo].[ProductCategory]
                           ([ProductCategoryName])
                        VALUES
                           (@ProductCategoryName)";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@ProductCategoryName", category);
            int result = cmd.ExecuteNonQuery();

            connection.Close();

            Console.WriteLine(result == 1 ? "Creating Successful" : "Creating Failed");

        }

        // Read Product Categories
        public void ReadProductCategories()
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            connection.Open();

            string query = @"SELECT [ProductCategoryId]
                            ,[ProductCategoryName]
                            FROM [dbo].[ProductCategory]";

            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            connection.Close();

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

            SqlConnection connection = new SqlConnection(_connectionString);

            connection.Open();

            string query = @"UPDATE [dbo].[ProductCategory]
                           SET [ProductCategoryName] = @ProductCategoryName
                         WHERE ProductCategoryId = @ProductCategoryId";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@ProductCategoryId", id);
            cmd.Parameters.AddWithValue("@ProductCategoryName", category);
            int result = cmd.ExecuteNonQuery();

            Console.WriteLine(result == 1 ? "Updating Successful" : "Updating Failed");

            connection.Close();
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

            SqlConnection connection = new SqlConnection(_connectionString);

            connection.Open();

            string query = @"DELETE FROM [dbo].[ProductCategory]
                                WHERE ProductCategoryId = @Id";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", id);
            int result = cmd.ExecuteNonQuery();
            Console.WriteLine(result > 0 ? "Deleting Successfully" : "Deleting Failed.");

            connection.Close();
        }
    }
}
