using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.RegularExpressions;

namespace Views.Controllers
{
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

        [HttpGet("/ssrs-proxy/{**ssrsPath}")]
        [HttpGet("/ReportServer/{**ssrsPath}")]
        [HttpPost("/ssrs-proxy/{**ssrsPath}")]
        [HttpPost("/ReportServer/{**ssrsPath}")]
        [HttpGet("/ScriptResource.axd")]
        [HttpGet("/WebResource.axd")]
        [HttpPost("/ScriptResource.axd")]
        [HttpPost("/WebResource.axd")]
        [HttpGet("/Reserved.ReportViewerWebControl.axd")]
        [HttpPost("/Reserved.ReportViewerWebControl.axd")]
        [HttpGet("/ai.2.min.js")]
        [IgnoreAntiforgeryToken]
        public async Task Proxy(string? ssrsPath)
        {
            // USE LOW-LEVEL RESPONSE ACCESS TO BYPASS MIDDLEWARE INTERFERENCE
            
            if (string.IsNullOrEmpty(ssrsPath))
            {
                ssrsPath = Request.Path.Value?.TrimStart('/') ?? string.Empty;
            }

            if (ssrsPath.Equals("ssrs-proxy", StringComparison.OrdinalIgnoreCase) || ssrsPath.StartsWith("ssrs-proxy/", StringComparison.OrdinalIgnoreCase))
                ssrsPath = ssrsPath.Length == 10 ? string.Empty : ssrsPath.Substring(11);
            
            // 1. RE-ROOTING LOGIC
            // SSRS often generates relative links (e.g. "Reserved.ReportViewerWebControl.axd" or "?rs:ImageID=...")
            // When these are inside an iframe at a deep path (e.g. /ssrs-proxy/Folder/Report), the browser
            // resolves them to /ssrs-proxy/Folder/Reserved... or /ssrs-proxy/Folder/?rs:ImageID=...
            // We must strip the "Folder/" prefix to hit the root SSRS endpoints.
            string query = Request.QueryString.Value ?? string.Empty;
            
            bool isGlobalHandler = ssrsPath.Contains(".axd", StringComparison.OrdinalIgnoreCase) ||
                                   ssrsPath.Contains(".css", StringComparison.OrdinalIgnoreCase) ||
                                   ssrsPath.Contains(".js", StringComparison.OrdinalIgnoreCase) ||
                                   ssrsPath.Contains(".png", StringComparison.OrdinalIgnoreCase) ||
                                   ssrsPath.Contains(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                   ssrsPath.Contains(".gif", StringComparison.OrdinalIgnoreCase);
            
            bool isGlobalCommand = query.Contains("rs:ImageID", StringComparison.OrdinalIgnoreCase) ||
                                  query.Contains("rs:SessionID", StringComparison.OrdinalIgnoreCase) ||
                                  query.Contains("Operation=GetResource", StringComparison.OrdinalIgnoreCase) ||
                                  query.Contains("OpType=Resource", StringComparison.OrdinalIgnoreCase);

            if ((isGlobalHandler || isGlobalCommand) && ssrsPath.Contains("/"))
            {
                 int lastSlash = ssrsPath.LastIndexOf('/');
                 if (isGlobalCommand && !isGlobalHandler)
                 {
                     // For pure command-based resources like ?rs:ImageID, forward to the root ReportServer
                     ssrsPath = string.Empty; 
                 }
                 else
                 {
                     // For file-based resources like Reserved.ReportViewerWebControl.axd, take just the filename
                     ssrsPath = ssrsPath.Substring(lastSlash + 1);
                 }
                 Console.WriteLine($"[SSRS Proxy] RE-ROOTED deep resource request to: {ssrsPath}");
            }
            else if (ssrsPath.Contains("ReportServer", StringComparison.OrdinalIgnoreCase))
            {
                // Legacy deep-path logic
                int rsIdx = ssrsPath.IndexOf("ReportServer", StringComparison.OrdinalIgnoreCase);
                ssrsPath = (ssrsPath.Length <= rsIdx + 12) ? string.Empty : ssrsPath.Substring(rsIdx + 13);
            }
            else if (ssrsPath.Equals("ReportServer", StringComparison.OrdinalIgnoreCase) || ssrsPath.StartsWith("ReportServer/", StringComparison.OrdinalIgnoreCase))
                ssrsPath = ssrsPath.Length == 12 ? string.Empty : ssrsPath.Substring(13);

            // AJAX Detection
            bool isAjax = Request.Headers["X-MicrosoftAjax"].ToString()
                              .Contains("Delta=true", StringComparison.OrdinalIgnoreCase) ||
                          Request.QueryString.Value?.Contains("Delta=true", StringComparison.OrdinalIgnoreCase) == true ||
                          Request.Headers.ContainsKey("X-Requested-With");

            Console.WriteLine($"[SSRS Proxy] {Request.Method} request to: {Request.Path}{Request.QueryString} (IsAjax: {isAjax})");

            var ssrsUser = _configuration["SSRS:User"];
            var ssrsPassword = _configuration["SSRS:Password"];
            var ssrsDomain = _configuration["SSRS:Domain"];
            var ssrsBaseUrl = (_configuration["SSRS:BaseUrl"] ?? "http://10.200.90.30").TrimEnd('/');
            var useWindowsAuth = _configuration.GetValue<bool>("SSRS:UseWindowsAuth");

            string fullUrl;
            bool isResourceHandler = ssrsPath.Contains(".axd", StringComparison.OrdinalIgnoreCase);
            bool isPages = ssrsPath.StartsWith("Pages/", StringComparison.OrdinalIgnoreCase);

            if (isResourceHandler || isPages)
            {
                fullUrl = $"{ssrsBaseUrl}/ReportServer/{ssrsPath.TrimStart('/')}{query}";
            }
            else if (string.IsNullOrEmpty(ssrsPath))
            {
                fullUrl = $"{ssrsBaseUrl}/ReportServer{query}";
            }
            else
            {
                string trimmedPath = ssrsPath.Trim('/');
                fullUrl = $"{ssrsBaseUrl}/ReportServer?/{trimmedPath}{query.Replace("?", "&")}";
            }

            Console.WriteLine($"[SSRS Proxy] Forwarding to: {fullUrl}");
            Console.WriteLine(fullUrl);

            // Production: UseWindowsAuth=true → app pool Windows identity forwarded (no popup).
            // Dev/Test:   UseWindowsAuth=false → explicit credentials from config.
            var clientHandler = new HttpClientHandler
            {
                UseDefaultCredentials = useWindowsAuth,
                Credentials = useWindowsAuth
                    ? null
                    : new NetworkCredential(ssrsUser, ssrsPassword, ssrsDomain),
                PreAuthenticate = true,
                AllowAutoRedirect = false,
                UseCookies = false,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            try
            {
                using var client = new HttpClient(clientHandler);
                var request = new HttpRequestMessage(new HttpMethod(Request.Method), fullUrl);

                // Header Proxying
                var restrictedHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase) 
                { 
                    "Content-Type", "Content-Length", "Host", "Connection", "Accept-Encoding", "Referer", "Origin"
                };

                foreach (var header in Request.Headers)
                {
                    if (!restrictedHeaders.Contains(header.Key))
                        request.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }

                var ssrsUri = new Uri(ssrsBaseUrl);
                request.Headers.Host = ssrsUri.Authority;
                
                string? incomingReferer = Request.Headers["Referer"].ToString();
                if (!string.IsNullOrEmpty(incomingReferer))
                {
                    string mappedReferer = incomingReferer.Replace(Request.Host.ToString(), ssrsUri.Host, StringComparison.OrdinalIgnoreCase);
                    mappedReferer = mappedReferer.Replace("/ssrs-proxy", "/ReportServer", StringComparison.OrdinalIgnoreCase);
                    request.Headers.TryAddWithoutValidation("Referer", mappedReferer);
                }
                else
                {
                    request.Headers.Referrer = new Uri(ssrsBaseUrl + "/ReportServer");
                }

                if (!string.IsNullOrEmpty(Request.Headers["Origin"]))
                {
                    request.Headers.TryAddWithoutValidation("Origin", ssrsBaseUrl);
                }

                if (Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
                {
                    using var ms = new MemoryStream();
                    await Request.Body.CopyToAsync(ms);
                    var bodyBytes = ms.ToArray();
                    
                    if (bodyBytes.Length > 0)
                    {
                        string bodyPreview = System.Text.Encoding.UTF8.GetString(bodyBytes.Take(100).ToArray());
                        Console.WriteLine($"[SSRS Proxy] POST Body ({bodyBytes.Length} bytes): {bodyPreview}");
                    }
                    
                    request.Content = new ByteArrayContent(bodyBytes);
                    if (Request.ContentType != null)
                        request.Content.Headers.TryAddWithoutValidation("Content-Type", Request.ContentType);
                }

                var response = await client.SendAsync(request);
                Response.StatusCode = (int)response.StatusCode;
                
                var contentType = response.Content.Headers.ContentType?.ToString() ?? "text/html";
                var contentLength = response.Content.Headers.ContentLength ?? 0;

                Console.WriteLine($"[SSRS Proxy] Received: {Response.StatusCode} | {contentType} | {contentLength} bytes");

                // ... (rest of header proxying)

                // 5. Response Header Proxying
                var transportHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "Content-Encoding", "Transfer-Encoding", "Vary", "Server", "X-Powered-By", "Content-Length",
                    // Strip framing-restriction headers — browser enforces these even inside a proxy,
                    // causing the iframe to silently render blank.
                    "X-Frame-Options",
                    "Content-Security-Policy",
                    "X-Content-Security-Policy",
                    "Feature-Policy",
                    "Permissions-Policy"
                };
                
                // ... (rest of header proxying remains same, but I need to make sure I don't break the loop)
                foreach (var header in response.Headers)
                {
                    if (transportHeaders.Contains(header.Key)) continue;

                    if (header.Key.Equals("Location", StringComparison.OrdinalIgnoreCase))
                    {
                        var location = header.Value.FirstOrDefault();
                        if (!string.IsNullOrEmpty(location) && location.Contains(ssrsBaseUrl, StringComparison.OrdinalIgnoreCase))
                        {
                            var updatedLocation = location.Replace(ssrsBaseUrl + "/ReportServer", "/ssrs-proxy", StringComparison.OrdinalIgnoreCase);
                            updatedLocation = updatedLocation.Replace(ssrsBaseUrl, "/ssrs-proxy", StringComparison.OrdinalIgnoreCase);
                            Response.Headers["Location"] = updatedLocation;
                        }
                        else
                        {
                            Response.Headers[header.Key] = header.Value.ToArray();
                        }
                    }
                    else if (header.Key.Equals("Set-Cookie", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (var cookie in header.Value)
                        {
                            string updatedCookie = cookie;

                            // FIX 1: Strip domain= so the cookie binds to the proxy domain
                            // (not 10.200.90.30), otherwise the browser never sends the SSRS
                            // session cookie back to the proxy and SSRS sees every postback
                            // as a brand-new visitor, causing full-page HTML fallbacks.
                            if (updatedCookie.Contains("domain=", StringComparison.OrdinalIgnoreCase))
                            {
                                int domIdx = updatedCookie.IndexOf("domain=", StringComparison.OrdinalIgnoreCase);
                                int domSemi = updatedCookie.IndexOf(";", domIdx);
                                updatedCookie = domSemi == -1
                                    ? updatedCookie.Substring(0, domIdx).TrimEnd(';', ' ')
                                    : updatedCookie.Substring(0, domIdx).TrimEnd(';', ' ') + "; " + updatedCookie.Substring(domSemi + 1).TrimStart();
                            }

                            // Rewrite path= to / so it covers all proxy routes
                            if (updatedCookie.Contains("path=", StringComparison.OrdinalIgnoreCase))
                            {
                                int pathIndex = updatedCookie.IndexOf("path=", StringComparison.OrdinalIgnoreCase);
                                int nextSemicolon = updatedCookie.IndexOf(";", pathIndex);
                                if (nextSemicolon == -1)
                                    updatedCookie = updatedCookie.Substring(0, pathIndex) + "path=/";
                                else
                                    updatedCookie = updatedCookie.Substring(0, pathIndex) + "path=/" + updatedCookie.Substring(nextSemicolon);
                            }
                            else
                            {
                                updatedCookie += "; path=/";
                            }

                            Console.WriteLine($"[SSRS Proxy] Set-Cookie → {updatedCookie}");
                            Response.Headers.Append("Set-Cookie", updatedCookie);
                        }
                    }
                    else
                    {
                        Response.Headers[header.Key] = header.Value.ToArray();
                    }
                }

                foreach (var header in response.Content.Headers)
                {
                    if (!transportHeaders.Contains(header.Key) && header.Key != "Content-Type")
                        Response.Headers[header.Key] = header.Value.ToArray();
                }

                Response.ContentType = contentType;
                Response.Headers["Content-Encoding"] = "identity";
                Response.Headers["X-Content-Type-Options"] = "nosniff";

                bool shouldRewrite = contentType.Contains("text/html") || 
                                   contentType.Contains("text/plain") ||
                                   contentType.Contains("javascript") || 
                                   contentType.Contains("text/css");
                
                if (contentType.Contains("font") || contentType.Contains("image") || fullUrl.Contains(".woff", StringComparison.OrdinalIgnoreCase))
                    shouldRewrite = false;

                if (shouldRewrite)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    
                    if (fullUrl.Contains("ConsignmentNoteReport") && !isAjax)
                    {
                        try {
                            System.IO.File.WriteAllText(@"c:\Swati\Projects\Updated ARMS\ARMS.v2\ARMS_Reports\ARMS_Reports\consignment_debug.html", content);
                        } catch { }
                    }
                    
                    if (!isAjax)
                    {
                         string trace = content.Length > 1000 ? content.Substring(0, 1000) : content;
                         Console.WriteLine($"[SSRS Proxy] CONTENT TRACE [{Response.StatusCode}] (First 1000 of {content.Length} chars):\n{trace}");
                         
                         if (content.Contains("rsProcessingAborted", StringComparison.OrdinalIgnoreCase))
                             Console.WriteLine("[SSRS Proxy] !!! DETECTED SSRS PROCESSING ABORTED !!!");
                         if (content.Contains("An error occurred during report processing", StringComparison.OrdinalIgnoreCase))
                             Console.WriteLine("[SSRS Proxy] !!! DETECTED REPORT PROCESSING ERROR !!!");
                         if (content.Contains("0 of 0", StringComparison.OrdinalIgnoreCase))
                             Console.WriteLine("[SSRS Proxy] !!! DETECTED 0 of 0 PAGES (Empty Result) !!!");
                         if (content.Contains("logon", StringComparison.OrdinalIgnoreCase) || content.Contains("login", StringComparison.OrdinalIgnoreCase))
                             Console.WriteLine("[SSRS Proxy] !!! DETECTED LOGIN/LOGON PAGE — credentials may be wrong !!!");
                    }

                    // 6. AJAX HANDLING
                    // ─────────────────────────────────────────────────────────────────────────────
                    // SSRS uses ASP.NET UpdatePanel / ScriptManager for ALL form posts (including the
                    // initial rs:Command=Render). This means:
                    //   - The POST has X-MicrosoftAjax: Delta=true
                    //   - SSRS returns a full HTML page (not ScriptManager delta format)
                    //   - Standard ScriptManager JS throws PageRequestManagerParserErrorException
                    //
                    // ROOT FIX: inject a script into the VIEWER SHELL HTML (returned by the initial GET)
                    // that disables UpdatePanel partial rendering. Without it, subsequent form submits
                    // become regular full POSTs (no X-MicrosoftAjax header). The proxy handles those
                    // as non-AJAX, rewrites URLs, and returns the full rendered report HTML → iframe
                    // naturally navigates to show the data. No Delta format / session dependency.
                    //
                    // Cases that still need silencing:
                    //   C) Non-HTML 4xx error payload for any remaining AJAX call.
                    //   D) Navigation full-HTML fallback (session lost) for non-render Delta calls.
                    // ─────────────────────────────────────────────────────────────────────────────
                    bool isAjaxFullHtmlFallback = false;
                    if (isAjax)
                    {
                        string trimmed = content.TrimStart();
                        bool isFullDoc = trimmed.StartsWith("<!DOCTYPE", StringComparison.OrdinalIgnoreCase) ||
                                          trimmed.StartsWith("<html", StringComparison.OrdinalIgnoreCase);

                        if (Response.StatusCode >= 400 && !isFullDoc)
                        {
                            // Case C: non-HTML error → silence
                            Console.WriteLine($"[SSRS Proxy] SILENCING AJAX Error ({Response.StatusCode}) for {fullUrl}");
                            Response.StatusCode = 200;
                            Response.ContentType = "text/plain";
                            Response.Headers["Content-Encoding"] = "identity";
                            await Response.WriteAsync("");
                            return;
                        }

                        if (isFullDoc)
                        {
                            // Case D: any AJAX call that returned full HTML.
                            // Silence it — if the UpdatePanel-disabler script was injected into the
                            // viewer shell, SSRS form submits will no longer be Delta POSTs so this
                            // branch should never fire for report renders.
                            Console.WriteLine($"[SSRS Proxy] SILENCING AJAX full-HTML ({content.Length} bytes) for {fullUrl}");
                            Response.StatusCode = 200;
                            Response.ContentType = "text/plain";
                            Response.Headers["Content-Encoding"] = "identity";
                            await Response.WriteAsync("");
                            return;
                        }
                    }

                    // 7. Apply URL Replacements
                    // For true AJAX partial-update responses (ScriptManager length-prefixed format)
                    // we must NOT rewrite content because changing string length breaks the protocol.
                    // We DO rewrite for all non-AJAX responses.
                    bool shouldApplyRewrites = !isAjax || isAjaxFullHtmlFallback;

                    if (shouldApplyRewrites && !string.IsNullOrEmpty(ssrsBaseUrl))
                    {
                        content = content.Replace(ssrsBaseUrl + "/ReportServer", "/ssrs-proxy");
                        content = content.Replace(ssrsBaseUrl, "/ssrs-proxy");
                    }
                    
                    if (shouldApplyRewrites)
                    {
                        // 1. Hostname-agnostic absolute and root-relative replacements
                        // Pattern: optional (http/https://hostname), followed by /ReportServer
                        string urlPattern = @"((?:http|https)://[a-zA-Z0-9\.\-:]+)?/ReportServer";
                        content = Regex.Replace(content, urlPattern, "/ssrs-proxy", RegexOptions.IgnoreCase);

                        // 2. Handler and global resource rewrites (Reserved.axd, etc.)
                        // Force these to be root-relative to /ssrs-proxy/ so they bypass deep-path routing issues
                        string handlerPattern = @"(?<=[ '""\(])(?:/ReportServer/)?(Reserved\.ReportViewerWebControl\.axd|ScriptResource\.axd|WebResource\.axd|Styles/|Images/|js/)(?=[/\?\w])";
                        content = Regex.Replace(content, handlerPattern, "/ssrs-proxy/$1", RegexOptions.IgnoreCase);

                        // 3. Command-based relative replacements (ReportServer? or ReportServer/)
                        string relativePattern = @"(?<=[ '""\(])ReportServer(?=[/\?])";
                        content = Regex.Replace(content, relativePattern, "/ssrs-proxy", RegexOptions.IgnoreCase);

                        // 4. Configuration object rewrites
                        content = content.Replace("\"/ReportServer\"", "\"/ssrs-proxy\"");
                        content = content.Replace("'/ReportServer'", "'/ssrs-proxy'");

                        // 5. Inject UpdatePanel disabler to prevent SSRS from hanging on AJAX requests
                        if (content.IndexOf("</body>", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            string disablerScript = @"
<script>
    window.addEventListener('load', function() {
        if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
            Sys.WebForms.PageRequestManager.getInstance()._updatePanelIDs = [];
        }
    });
</script>";
                            content = System.Text.RegularExpressions.Regex.Replace(content, "</body>", disablerScript + "</body>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        }
                    }


                    await Response.WriteAsync(content);
                }
                else
                {
                    // For non-rewritable streams (images, fonts), copy directly
                    await response.Content.CopyToAsync(Response.Body);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SSRS Proxy] ERROR [{fullUrl}]: {ex.Message}");
                if (!Response.HasStarted)
                {
                    Response.StatusCode = 200; 
                    Response.ContentType = "text/plain";
                    Response.Headers["Content-Encoding"] = "identity";
                    await Response.WriteAsync(isAjax ? "" : $"Proxy error: {ex.Message}");
                }
            }
        }
    }
}
