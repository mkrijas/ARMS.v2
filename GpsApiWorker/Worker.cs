using System.Text.Json;
using ArmsModels.BaseModels;
using ArmsServices.DataServices;


namespace GpsApiWorker
{

    public class ApiPollingService(ILogger<ApiPollingService> _logger, IHttpClientFactory _clientFactory,ITelemetryService _truckService) : BackgroundService
    {    
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("API Polling Service running.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    //await CallApiAsync();
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                // Wait for the specified interval before the next call
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); // Adjust interval as needed
            }
        }

        private async Task CallApiAsync()
        {
            // Create a client for your API calls
            var httpClient = _clientFactory.CreateClient("MyApi");
            var response = await httpClient.GetAsync("api/data");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("API call successful. Content length: {Length}", content.Length);

                try
                {
                    List<TelemetryModel> items = JsonSerializer.Deserialize<List<TelemetryModel>>(content) ?? new List<TelemetryModel>();
                    int? res = _truckService.UpdateTelemetry(items);
                    _logger.LogInformation("Updated to database successfully");
                }
                catch (JsonException jsonEx)
                {
                    _logger.LogError(jsonEx, "Failed to deserialize API response.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while processing the API response.");
                }
            }
            else
            {
                _logger.LogWarning("API call failed with status code: {StatusCode}", response.StatusCode);
            }
        }
    } 


}
