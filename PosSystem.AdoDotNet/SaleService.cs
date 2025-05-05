using Microsoft.Data.SqlClient;
using PosSystem.AdoDotNet.Shared;
using System;
using System.Collections.Generic;
using System.Data;


namespace PosSystem.AdoDotNet;

public class SaleService
{
    public readonly AdoDotNetService _adoDotNetService;

    public SaleService()
    {
        _adoDotNetService = new AdoDotNetService(AppSetting.ConnectionString);
    }

    public void CreateSale()
    {
        string voucherNo = "V" + DateTime.Now.ToString("yyyyMMddHHmmss");
        DateTime saleDate = DateTime.Now;
        List<(int ProductId, int Quantity, decimal Price)> saleDetails = new List<(int, int, decimal)>();
        decimal totalAmount = 0;

        while (true)
        {
            Console.Write("Enter Product ID (or 0 to finish): ");
            if (!int.TryParse(Console.ReadLine(), out int productId) || productId < 0)
            {
                Console.WriteLine("Invalid Product ID.");
                continue;
            }

            if (productId == 0)
            {
                break;
            }

            string productQuery = "SELECT COUNT(*) FROM Product WHERE ProductId = @ProductId";
            using (SqlConnection connection = new SqlConnection(AppSetting.ConnectionString))
            {
                connection.Open();

                using (SqlCommand checkCmd = new SqlCommand(productQuery, connection))
                {
                    checkCmd.Parameters.AddWithValue("@ProductId", productId);
                    object result = checkCmd.ExecuteScalar();
                    int exists = (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : 0;

                    if (exists == 0)
                    {
                        Console.WriteLine("Product ID does not exist.");
                        continue;
                    }
                }
            }

            Console.Write("Enter Quantity: ");
            /*if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
            {
                continue;
            }
            else
            {
                Console.WriteLine("Invalid quantity. Please enter a integer value.");
            }*/
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Invalid quantity. Please enter a positive integer.");
                continue;
            }

            // Fetch product price
            decimal price = 0;
            using (SqlConnection connection = new SqlConnection(AppSetting.ConnectionString))
            {
                connection.Open();
                string priceQuery = "SELECT Price FROM Product WHERE ProductId = @ProductId";
                using (SqlCommand cmd = new SqlCommand(priceQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    object result = cmd.ExecuteScalar();
                    if (result == null)
                    {
                        Console.WriteLine("Product not found.");
                        continue;
                    }
                    price = Convert.ToDecimal(result);
                }
            }

            saleDetails.Add((productId, quantity, price));
            totalAmount += price * quantity;
        }

        if (saleDetails.Count == 0)
        {
            Console.WriteLine("No sale items entered. Sale not recorded.");
            return;
        }

        // Insert Sale and SaleDetail with transaction
        using (SqlConnection connection = new SqlConnection(AppSetting.ConnectionString))
        {
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                // Insert into Sale and get SaleId
                string saleQuery = @"INSERT INTO [dbo].[Sale]
                                    ([VoucherNo], [SaleDate], [TotalAmount])
                                    VALUES (@VoucherNo, @SaleDate, @TotalAmount);
                                    SELECT SCOPE_IDENTITY();";

                int saleId;
                using (SqlCommand cmd = new SqlCommand(saleQuery, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@VoucherNo", voucherNo);
                    cmd.Parameters.AddWithValue("@SaleDate", saleDate);
                    cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                    saleId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // Insert into SaleDetail
                foreach (var item in saleDetails)
                {
                    string detailQuery = @"INSERT INTO [dbo].[SaleDetail]
                                           ([SaleId], [ProductId], [Quantity], [Price], [VoucherNo])
                                           VALUES (@SaleId, @ProductId, @Quantity, @Price, @VoucherNo)";

                    using (SqlCommand cmd = new SqlCommand(detailQuery, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@SaleId", saleId);
                        cmd.Parameters.AddWithValue("@ProductId", item.ProductId);
                        cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                        cmd.Parameters.AddWithValue("@Price", item.Price);
                        cmd.Parameters.AddWithValue("@VoucherNo", voucherNo);
                        cmd.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
                Console.WriteLine("Sale recorded successfully with VoucherNo: " + voucherNo);
            }
            catch (Exception ex)    
            {
                transaction.Rollback();
                Console.WriteLine("Error occurred: " + ex.Message);
            }
        }
    }

    public void ReadSales()
    {
        string query = @"SELECT s.VoucherNo, s.SaleDate, s.TotalAmount, 
                         p.ProductName, sd.Quantity, sd.Price
                         FROM Sale s
                         JOIN SaleDetail sd ON s.VoucherNo = sd.VoucherNo
                         JOIN Product p ON sd.ProductId = p.ProductId
                         ORDER BY s.VoucherNo, p.ProductName";

        var dt = _adoDotNetService.Query(query);

        if (dt.Rows.Count > 0)
        {
            string currentVoucher = "";
            foreach (DataRow row in dt.Rows)
            {
                string voucher = row["VoucherNo"].ToString();
                if (voucher != currentVoucher)
                {
                    currentVoucher = voucher;
                    Console.WriteLine($"\nVoucher No: {voucher}, Date: {Convert.ToDateTime(row["SaleDate"]):yyyy-MM-dd}, Total: {row["TotalAmount"]}");
                }

                Console.WriteLine($"Product: {row["ProductName"]}, Quantity: {row["Quantity"]}, Price: {row["Price"]}");
            }
        }
        else
        {
            Console.WriteLine("No sales found.");
        }
        
    }
}
