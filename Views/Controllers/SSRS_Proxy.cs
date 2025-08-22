using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using Microsoft.JSInterop;
using System.Security.Policy;
using Microsoft.Extensions.Configuration;


    namespace Views.Controllers
    {
        [Route("ssrs-proxy/{**ssrsPath}")]
        [ApiController]
        public class SsrsProxyController : ControllerBase
        {
            private readonly IHttpClientFactory _httpClientFactory;
            private readonly IConfiguration _configuration;

            public SsrsProxyController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
            {
                _httpClientFactory = httpClientFactory;
                _configuration = configuration;
            }

            [HttpGet]
            public async Task<IActionResult> Proxy(string ssrsPath)
            {
                if (string.IsNullOrWhiteSpace(ssrsPath))
                    return BadRequest("Invalid SSRS path.");

                // Get credentials from configuration
                var ssrsUser = _configuration["SSRS:User"];
                var ssrsPassword = _configuration["SSRS:Password"];
                var ssrsDomain = _configuration["SSRS:Domain"];
                var ssrsBaseUrl = _configuration["SSRS:BaseUrl"] ?? "http://10.200.90.30";                

                var trimmedPath = ssrsPath.TrimEnd('/');
                if (!trimmedPath.StartsWith("/"))
                    trimmedPath = "/" + trimmedPath;             

                var query =  Request.QueryString.Value ?? string.Empty;                   
                var fullUrl = $"{ssrsBaseUrl}/ReportServer?{trimmedPath}{query.Replace("?", "&")}";

            var clientHandler = new HttpClientHandler
                {
                    UseDefaultCredentials = false,
                    PreAuthenticate = true,
                    Credentials = new NetworkCredential(ssrsUser, ssrsPassword, ssrsDomain)
                };

                try
                {
                    using var client = new HttpClient(clientHandler);
                    var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);
                    var response = await client.SendAsync(request);

                    Console.WriteLine($"Resolved SSRS Path: {ssrsPath}");
                    Console.WriteLine($"Resolved Full URL: {fullUrl}");

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorText = await response.Content.ReadAsStringAsync();
                        return Content(errorText, "text/html");
                    }

                    var contentType = response.Content.Headers.ContentType?.ToString() ?? "text/html";

                    if (contentType.StartsWith("text/html"))
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        content = content.Replace("src=\"/", $"src=\"{ssrsBaseUrl}/");
                        content = content.Replace("href=\"/", $"href=\"{ssrsBaseUrl}/");
                        content = content.Replace("action=\"/", $"action=\"{ssrsBaseUrl}/");
                        return Content(content, contentType);
                    }

                    var fileName = response.Content.Headers.ContentDisposition?.FileName?.Trim('"') ?? "report";
                    var stream = await response.Content.ReadAsStreamAsync();
                    return File(stream, contentType, fileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error proxying SSRS request: {ex.Message}");
                    return StatusCode(500, "Error proxying SSRS request.");
                }
            }
        }
    }

