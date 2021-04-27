using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices
{
    public interface IDbService
    {
        SqlDataReader GetDataReader(string procedureName, List<SqlParameter> parameters);
        int ExecuteNonQuery(string procedureName, List<SqlParameter> parameters);
    }

    public class DbService:IDbService
    {
        protected string ConnectionString { get; set; }
        public DbService(IConfiguration configuration)
        {
            this.ConnectionString = configuration.GetConnectionString("ArmsDB");
        }
        private SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();
            return connection;
        }

        public SqlDataReader GetDataReader(string procedureName, List<SqlParameter> parameters)
        {
            SqlDataReader dr;
            try
            {
                SqlConnection connection = GetConnection();
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procedureName;

                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return dr;
        }


        public int ExecuteNonQuery(string procedureName, List<SqlParameter> parameters)
        {
            int rows;
            try
            {
                SqlConnection connection = GetConnection();
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procedureName;

                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }
                    rows = cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return rows;
        }
    }
}
