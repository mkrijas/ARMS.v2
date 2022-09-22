using BoldReports.Web.ReportViewer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.IO;

namespace BlazorReportingTools.Data
{
    [Route("api/{controller}/{action}/{id?}")]
    public class BoldReportsAPIController : ControllerBase, IReportController
    {
        // Report viewer requires a memory cache to store the information of consecutive client requests and
        // the rendered report viewer in the server.
        private IMemoryCache _cache;

        // IWebHostEnvironment used with sample to get the application data from wwwroot.
        private IWebHostEnvironment _hostingEnvironment;

        public BoldReportsAPIController(IMemoryCache memoryCache, IWebHostEnvironment hostingEnvironment)
        {
            _cache = memoryCache;
            _hostingEnvironment = hostingEnvironment;
        }
        //Get action for getting resources from the report
        [ActionName("GetResource")]
        [AcceptVerbs("GET")]
        // Method will be called from Report Viewer client to get the image src for Image report item.
        public object GetResource(ReportResource resource)
        {
            return ReportHelper.GetResource(resource, this, _cache);
        }

        // Method will be called to initialize the report information to load the report with ReportHelper for processing.
        [NonAction]
        public void OnInitReportOptions(ReportViewerOptions reportOption)
        {
            reportOption.ReportModel.ReportServerUrl = @"http://10.200.50.39:80/ReportServer";            
            reportOption.ReportModel.ProcessingMode = ProcessingMode.Remote;
            reportOption.ReportModel.ReportServerCredential = new System.Net.NetworkCredential("TEAMTHAI\\RIJASMK", "Ri*tt012");
            
            //BoldReports.Web.DataSourceCredentials dc1 = new BoldReports.Web.DataSourceCredentials("MyDB", "TEAMTHAI\\RIJASMK", "Ri*tt012");

            //reportOption.ReportModel.DataSourceCredentials = new List<BoldReports.Web.DataSourceCredentials>()
            //{
            //    dc1
            //};

            string basePath = _hostingEnvironment.WebRootPath;
            // Here, we have loaded the sales-order-detail.rdl report from the application folder wwwroot\Resources. sales-order-detail.rdl should be in the wwwroot\Resources application folder.
            //System.IO.FileStream inputStream = new System.IO.FileStream(basePath + @"\resources\" + reportOption.ReportModel.ReportPath + ".rdl", System.IO.FileMode.Open, System.IO.FileAccess.Read);
            //System.IO.FileStream inputStream = new System.IO.FileStream(reportOption.ReportModel.ReportServerUrl + reportOption.ReportModel.ReportPath + ".rdl", System.IO.FileMode.Open, System.IO.FileAccess.Read);

            //MemoryStream reportStream = new MemoryStream();
            //inputStream.CopyTo(reportStream);
            //reportStream.Position = 0;
            //inputStream.Close();
            //reportOption.ReportModel.Stream = reportStream;
        }

        // Method will be called when report is loaded internally to start the layout process with ReportHelper.
        [NonAction]
        public void OnReportLoaded(ReportViewerOptions reportOption)
        {
        }

        [HttpPost]
        public object PostFormReportAction()
        {
            return ReportHelper.ProcessReport(null, this, _cache);
        }

        // Post action to process the report from the server based on json parameters and send the result back to the client.
        [HttpPost]
        public object PostReportAction([FromBody] Dictionary<string, object> jsonArray)
        {
            return ReportHelper.ProcessReport(jsonArray, this, this._cache);
        }       
    }

}