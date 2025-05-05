using Microsoft.Data.SqlClient;
using System.Data;

namespace PosSystem.AdoDotNet.Shared;

public class AdoDotNetService
    {
        private readonly string _connectionString;

        public AdoDotNetService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable Query(string query, params SqlParameterModel[] sqlParameters)
        {
            SqlConnection connection = new SqlConnection( _connectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand(query,connection);
            foreach (var sqlParameter in sqlParameters)
            {
                cmd.Parameters.AddWithValue(sqlParameter.Name, sqlParameter.Value);
            }

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            connection.Close();

            return dt;

        }

        public int Execute(string query, params SqlParameterModel[] sqlParameters)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand(query, connection);
            foreach (var sqlParameter in sqlParameters)
            {
                cmd.Parameters.AddWithValue(sqlParameter.Name, sqlParameter.Value);
            }

            int result = cmd.ExecuteNonQuery();

            connection.Close();

            return result;

        }

    /*public void ExecuteTransaction(string query, params SqlParameterModel[] sqlParameters)
    {
        SqlConnection connection = new SqlConnection(_connectionString);

        connection.Open();
        SqlTransaction transaction = connection.BeginTransaction();
        try
        {
            foreach (var sqlParameter in sqlParameters)
            {
                using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
                {
                    foreach (var param in sqlParameters)
                    {
                        cmd.Parameters.AddWithValue(param.Name, param.Value ?? DBNull.Value);
                    }

                    cmd.ExecuteNonQuery();
                }
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }*/

}

public class SqlParameterModel
{
    public string Name { get; set; }
    public object Value { get; set; }
}
