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
        IAsyncEnumerable<IDataRecord> GetDataReader(string procedureName, List<SqlParameter> parameters);
        Task<int> ExecuteNonQuery(string procedureName, List<SqlParameter> parameters);
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
        public async IAsyncEnumerable<IDataRecord> GetDataReader(string procedureName, List<SqlParameter> parameters)
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
                        yield return (IDataRecord)dr;
                    }
                }
            }            
        }

        public async Task<int> ExecuteNonQuery(string procedureName, List<SqlParameter> parameters)
        {
            int rows;
            try
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
                    rows = await cmd.ExecuteNonQueryAsync();
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
