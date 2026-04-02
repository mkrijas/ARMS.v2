using ArmsModels.BaseModels;
using ArmsServices;
using ArmsServices.DataServices;
using Microsoft.AspNetCore.Authentication;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GpsApiWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add Web API Controller support
            builder.Services.AddControllers();
            builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new CustomDateTimeConverter());
            });
            builder.Services.AddHttpClient("MyApi", client =>
            {
                client.BaseAddress = new Uri("https://api.example.com"); // Set the base address of your API                
            });

            builder.Services.AddHostedService<ApiPollingService>();
            builder.Services.AddSingleton<IDbService,DbService>();
            builder.Services.AddSingleton<ITelemetryService, TelemetryService>();

            builder.Services.AddAuthentication("ApiKey").AddScheme<AuthenticationSchemeOptions, ApiKeyAuthHandler>("ApiKey", null);
            builder.Services.AddAuthorization();

            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();
            // Map the route handlers (ApiSubscriberController)
            app.MapControllers();
            
            app.Run();
        }
    }
   
}
