using ArmsModels.BaseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ArmsServices
{
    public interface IDbService
    {
        IAsyncEnumerable<IDataRecord> GetDataReaderAsync(string procedureName, List<SqlParameter> parameters);
        Task<int> ExecuteNonQueryAsync(string procedureName, List<SqlParameter> parameters);
        IEnumerable<IDataRecord> GetDataReader(string procedureName, List<SqlParameter> parameters);
        int ExecuteNonQuery(string procedureName, List<SqlParameter> parameters);
        IEnumerable<IDataRecord> QuerySql(string Query, List<SqlParameter> parameters);
        object ExecuteScalar(string procedureName, List<SqlParameter> parameters);
        void ChangeConnectionString(string _ConnectionString);
        string GetCurrentConnectionString();
    }

    public class DbService : IDbService
    {
        private string ConnectionString { get; set; }

        ILogger<DbService> _logger;
        IConfiguration _config;
        

        public DbService(IConfiguration configuration, ILogger<DbService> logger)
        {
            this._logger = logger;
            _config = configuration;

            //var userID =   usrmgr.GetUserId;
           

            this.ConnectionString = configuration.GetConnectionString("ArmsDB");
        }

        public void ChangeConnectionString(string _ConnectionString)
        {
            this.ConnectionString = _config.GetConnectionString(_ConnectionString) ;
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
            StringBuilder errorMessages = new StringBuilder();

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
            StringBuilder errorMessages = new StringBuilder();
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

        public IEnumerable<IDataRecord> QuerySql(string Query, List<SqlParameter> parameters)
        {
            StringBuilder errorMessages = new StringBuilder();

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(Query, connection))
                {
                    cmd.CommandType = CommandType.Text;
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

        public object ExecuteScalar(string procedureName, List<SqlParameter> parameters)
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
                return cmd.ExecuteScalar();
            }
        }

        public string GetCurrentConnectionString()
        {
            return ConnectionString;
        }
    }
}