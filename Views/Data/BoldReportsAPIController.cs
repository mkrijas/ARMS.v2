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
            reportOption.ReportModel.EnableVirtualEvaluation = true;
            reportOption.ReportModel.DisablePageSplitting = true;
            //reportOption.ReportModel.ReportServerUrl = "http://192.168.10.25:4456/Reports_SSRS"; 
            //reportOption.ReportModel.ReportServerCredential = new System.Net.NetworkCredential("MSSQLSERVER\\Administrator", "A2teamthai");
            //reportOption.ReportModel.ProcessingMode = ProcessingMode.Remote;
            //
            reportOption.ReportModel.ReportServerUrl = @"http://10.200.50.39:80/ReportServer";
            reportOption.ReportModel.ReportServerCredential = new System.Net.NetworkCredential("RIJASMK@TEAMTHAI.IN", "Ri*tt012");
            
            
            //BoldReports.Web.ReportParameter par1 = new() { Name = "branch_name", Values = new List<string>() { "CBE" } };
            //BoldReports.Web.ReportParameter par2 = new() { Name = "ocId", Values = new List<string>() { "1" } };
            //BoldReports.Web.ReportParameter par3 = new() { Name = "from", Values = new List<string>() { "08/08/2022" } };
            //BoldReports.Web.ReportParameter par4 = new() { Name = "to", Values = new List<string>() { "08/08/2022" } };
            //reportOption.ReportModel.Parameters = new List<BoldReports.Web.ReportParameter>() { par1, par2, par3, par4 };
            reportOption.ReportModel.DataSourceCredentials.Add(new BoldReports.Web.DataSourceCredentials("Mydb", "ars", "asd123."));


            //string basePath = _hostingEnvironment.WebRootPath;

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