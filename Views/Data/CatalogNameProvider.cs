using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using ArmsServices; 

namespace Views.Data
{
    public interface ICatalogNameProvider
    {
        string GetCatalogName();
    }

    public class CatalogNameProvider : ICatalogNameProvider
    {
        private readonly IConfiguration _configuration;
        private IDbService _db;
        public CatalogNameProvider(IConfiguration configuration,IDbService _service)
        {
            _configuration = configuration;
            _db = _service;
        }

        public string GetCatalogName()
        {
            var connectionString =  _db.GetCurrentConnectionString();           
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            return connectionStringBuilder.InitialCatalog;
        }
    }
}
