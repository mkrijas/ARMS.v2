using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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

        IAsyncEnumerable<IDataRecord> GetDataReaderAsync(string procedureName, List<SqlParameter> parameters);
        Task<int> ExecuteNonQueryAsync(string procedureName, List<SqlParameter> parameters);
        IEnumerable<IDataRecord> GetDataReader(string procedureName, List<SqlParameter> parameters);
        int ExecuteNonQuery(string procedureName, List<SqlParameter> parameters);
    }

    public class DbService:IDbService
    {
        private string ConnectionString { get; set; }
        ILogger<DbService> _logger;
        public DbService(IConfiguration configuration,ILogger<DbService> logger)
        {
            this._logger = logger;
            this.ConnectionString = configuration.GetConnectionString("ArmsDB");            
        }
        public async IAsyncEnumerable<IDataRecord> GetDataReaderAsync(string procedureName, List<SqlParameter> parameters)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }
                    SqlDataReader dr = null;
                    dr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            
                    while (await dr.ReadAsync())
                    {
                        yield return dr;
                    }
                }
            }            
        }

        public async Task<int> ExecuteNonQueryAsync(string procedureName, List<SqlParameter> parameters)
        {          
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procedureName;

                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }
                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            
        }

        public IEnumerable<IDataRecord> GetDataReader(string procedureName, List<SqlParameter> parameters)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(procedureName, connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }
                    SqlDataReader dr = null;
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    
                    while (dr.Read())
                    {
                        yield return dr;
                    }
                    dr.Close();
                }
            }
        }
        public int ExecuteNonQuery(string procedureName, List<SqlParameter> parameters)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = procedureName;                

                if (parameters != null && parameters.Count > 0)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }
                connection.Open();
                return cmd.ExecuteNonQuery();               
            }
        }
    }
}
