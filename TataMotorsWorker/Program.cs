using ArmsModels.BaseModels;
using ArmsServices;
using ArmsServices.DataServices;
using System.Text.Json;

namespace TataMotorsWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddControllers();

            builder.Services.Configure<OnethorApiSettings>(builder.Configuration.GetSection("OnethorApi"));
            builder.Services.AddHttpClient<OnethorApiClient>();

            builder.Services.AddHostedService<ApiPollingService>();
            builder.Services.AddSingleton<IDbService, DbService>();
            builder.Services.AddSingleton<ITelemetryService, TelemetryService>();

            var app = builder.Build();

            app.MapControllers();
            
            app.Run();
        }
    }
}
