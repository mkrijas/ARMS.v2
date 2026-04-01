using ArmsServices;
using ArmsServices.DataServices;

namespace GpsApiWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add Web API Controller support
            builder.Services.AddControllers();

            builder.Services.AddHttpClient("MyApi", client =>
            {
                client.BaseAddress = new Uri("https://api.example.com"); // Set the base address of your API                
            });

            builder.Services.AddHostedService<ApiPollingService>();
            builder.Services.AddSingleton<IDbService,DbService>();
            builder.Services.AddSingleton<ITelemetryService, TelemetryService>();

            var app = builder.Build();
            
            // Map the route handlers (ApiSubscriberController)
            app.MapControllers();
            
            app.Run();
        }
    }
}
