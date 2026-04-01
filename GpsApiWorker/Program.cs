namespace GpsApiWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHttpClient("MyApi", client =>
            {
                client.BaseAddress = new Uri("https://api.example.com"); // Set the base address of your API                
            });

            builder.Services.AddHostedService<ApiPollingService>();

            var host = builder.Build();
            host.Run();
        }
    }
}
