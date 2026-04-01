namespace GpsApiWorker
{

    public class ApiPollingService(ILogger<ApiPollingService> _logger, IHttpClientFactory _clientFactory) : BackgroundService
    {    
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("API Polling Service running.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CallApiAsync();
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
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("API call successful. Content length: {Length}", content.Length);
                // Process the API response here
            }
            else
            {
                _logger.LogWarning("API call failed with status code: {StatusCode}", response.StatusCode);
            }
        }
    } 
}
