using ArmsModels.BaseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


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
        Task<int> GetRecordCountAsync(string procedureName, List<SqlParameter> parameters);
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

            var connStr = configuration.GetConnectionString("ArmsDB");
            var builder = new SqlConnectionStringBuilder(connStr ?? string.Empty)
            {
                // Microsoft.Data.SqlClient defaults to encrypting connections. If the server
                // uses a certificate that isn't trusted (common in dev), allow trusting it.   
            };
            this.ConnectionString = builder.ToString();
        }

        public void ChangeConnectionString(string _ConnectionString)
        {
            var connStr = _config.GetConnectionString(_ConnectionString);
            var builder = new SqlConnectionStringBuilder(connStr ?? string.Empty)
            {
                TrustServerCertificate = true
            };
            this.ConnectionString = builder.ToString();
        }

        public async IAsyncEnumerable<IDataRecord> GetDataReaderAsync(string procedureName, List<SqlParameter> parameters)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                await connection.OpenAsync(SqlConnectionOverrides.OpenWithoutRetry, CancellationToken.None);
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
                await connection.OpenAsync(CancellationToken.None);
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public IEnumerable<IDataRecord> GetDataReader(string procedureName, List<SqlParameter> parameters)
        {
            StringBuilder errorMessages = new StringBuilder();

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open(SqlConnectionOverrides.OpenWithoutRetry);
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

        public async Task<int> GetRecordCountAsync(string procedureName, List<SqlParameter> parameters)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(procedureName, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        // Discover actual parameters to avoid "too many arguments" errors
                        try {
                            SqlCommandBuilder.DeriveParameters(cmd);
                        } catch (Exception ex) {
                             _logger.LogWarning($"[DbService] Could not derive parameters for {procedureName}: {ex.Message}. Falling back to manual assignment.");
                        }

                        if (parameters != null && parameters.Count > 0)
                        {
                            foreach (var p in parameters)
                            {
                                string pName = p.ParameterName.StartsWith("@") ? p.ParameterName : "@" + p.ParameterName;
                                
                                if (cmd.Parameters.Count > 0 && cmd.Parameters.Contains(pName))
                                {
                                    cmd.Parameters[pName].Value = p.Value ?? DBNull.Value;
                                }
                                else if (cmd.Parameters.Count == 0)
                                {
                                    // Fallback if DeriveParameters failed or wasn't supported
                                    cmd.Parameters.Add(new SqlParameter(pName, p.Value ?? DBNull.Value));
                                }
                            }
                        }

                        int count = 0;
                        using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                        {
                            do {
                                while (await dr.ReadAsync())
                                {
                                    count++;
                                    if (count > 1) break; // We only need enough to know it's not empty
                                }
                                if (count > 1) break;
                            } while (await dr.NextResultAsync());
                        }
                        return count;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[DbService] Error getting record count for {procedureName}");
                return -1; // Indicate error to trigger fallback
            }
        }

        public string GetCurrentConnectionString()
        {
            return ConnectionString;
        }
    }
}