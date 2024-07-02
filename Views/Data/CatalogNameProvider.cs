using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Views.Data
{
    public interface ICatalogNameProvider
    {
        string GetCatalogName();
    }

    public class CatalogNameProvider : ICatalogNameProvider
    {
        private readonly IConfiguration _configuration;

        public CatalogNameProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetCatalogName()
        {
            var connectionString = _configuration.GetConnectionString("ArmsDB");
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            return connectionStringBuilder.InitialCatalog;
        }
    }
}
