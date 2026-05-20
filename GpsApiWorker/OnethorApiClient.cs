using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace GpsApiWorker
{
    public class OnethorApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly OnethorApiSettings _settings;
        private readonly ILogger<OnethorApiClient> _logger;
        
        private static string? _cachedToken;
        private static DateTime _tokenExpiry = DateTime.MinValue;
        private static readonly SemaphoreSlim _tokenSemaphore = new SemaphoreSlim(1, 1);

        public OnethorApiClient(HttpClient httpClient, IOptions<OnethorApiSettings> settings, ILogger<OnethorApiClient> logger)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _logger = logger;

            if (!string.IsNullOrEmpty(_settings.BaseUrl))
            {
                _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
            }
        }

        private async Task<string> GetTokenAsync(CancellationToken cancellationToken)
        {
            if (_cachedToken != null && _tokenExpiry > DateTime.UtcNow)
            {
                return _cachedToken;
            }

            await _tokenSemaphore.WaitAsync(cancellationToken);
            try
            {
                // Double check pattern
                if (_cachedToken != null && _tokenExpiry > DateTime.UtcNow)
                {
                    return _cachedToken;
                }

                _logger.LogInformation("Requesting new auth token from Onethor API...");
                
                var requestContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("client_id", _settings.ClientId ?? string.Empty),
                    new KeyValuePair<string, string>("client_secret", _settings.ClientSecret ?? string.Empty),
                    new KeyValuePair<string, string>("grant_type", _settings.GrantType ?? "client_credentials")
                });

                var response = await _httpClient.PostAsync(_settings.TokenUrl, requestContent, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    var errContent = await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogError("Auth token request failed with status code {StatusCode}: {Error}", response.StatusCode, errContent);
                    throw new HttpRequestException($"Auth token request failed: {response.StatusCode} - {errContent}");
                }

                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                var tokenResp = JsonSerializer.Deserialize<TokenResponse>(json);
                
                if (tokenResp == null || string.IsNullOrEmpty(tokenResp.AccessToken))
                {
                    throw new Exception("Auth token response was invalid or empty.");
                }

                _cachedToken = tokenResp.AccessToken;
                
                int seconds = tokenResp.ExpiresIn;
                if (seconds <= 0)
                {
                    seconds = 300; // default 5 minutes
                }
                else if (seconds < 60)
                {
                    // If it is in minutes, convert to seconds
                    seconds *= 60;
                }

                // Buffer by 30 seconds
                _tokenExpiry = DateTime.UtcNow.AddSeconds(seconds - 30);
                
                _logger.LogInformation("Successfully acquired auth token. Expiry: {Expiry}", _tokenExpiry);
                return _cachedToken;
            }
            finally
            {
                _tokenSemaphore.Release();
            }
        }

        public async Task<List<VehicleTelemetry>> GetAllVehicleSnapshotsAsync(CancellationToken cancellationToken = default)
        {
            var token = await GetTokenAsync(cancellationToken);
            
            var allVehicles = new List<VehicleTelemetry>();
            int currentPage = 0;
            int pageSize = 50;
            int totalPages = 1;

            do
            {
                var requestUrl = $"{_settings.VehicleSnapshotUrl}?pageNumber={currentPage}&pageSize={pageSize}";
                
                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                _logger.LogInformation("Fetching vehicle snapshots page {Page}...", currentPage);
                var response = await _httpClient.SendAsync(request, cancellationToken);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errContent = await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogError("Failed to fetch vehicle snapshots on page {Page}. Status: {StatusCode}, Error: {Error}", currentPage, response.StatusCode, errContent);
                    response.EnsureSuccessStatusCode();
                }

                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                var result = JsonSerializer.Deserialize<AllVehicleTelemetryResponse>(json);

                if (result?.vehicles != null)
                {
                    allVehicles.AddRange(result.vehicles);
                    
                    if (result.pageable != null)
                    {
                        totalPages = result.pageable.totalPages;
                        _logger.LogInformation("Page {Page} fetched. Total elements: {TotalElements}, Total pages: {TotalPages}", 
                            currentPage, result.pageable.totalElements, totalPages);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }

                currentPage++;
                if (currentPage < totalPages)
                {
                    await Task.Delay(2500, cancellationToken);
                }
            } while (currentPage < totalPages);

            return allVehicles;
        }

        public async Task<VehicleTelemetry?> GetVehicleSnapshotAsync(string vehicleRegistrationNumber, CancellationToken cancellationToken = default)
        {
            var token = await GetTokenAsync(cancellationToken);
            var requestUrl = $"{_settings.VehicleSnapshotUrl}/{Uri.EscapeDataString(vehicleRegistrationNumber)}";
            
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            _logger.LogInformation("Fetching vehicle snapshot for {RegNo}...", vehicleRegistrationNumber);
            var response = await _httpClient.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("No data found for registration number: {RegNo}", vehicleRegistrationNumber);
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                var errContent = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError("Failed to fetch vehicle snapshot for {RegNo}. Status: {StatusCode}, Error: {Error}", vehicleRegistrationNumber, response.StatusCode, errContent);
                response.EnsureSuccessStatusCode();
            }

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<VehicleTelemetry>(json);
        }
    }
}
