using System.Collections.Generic;
using BoldReports.Web;

namespace Views.Data
{
    public class BoldReportViewerOptions
    {
        public string ReportPath { get; set; }
        public string ServiceURL { get; set; }
        public string ServerURL { get; set; }
        public string ServiceAuthorizationToken { get; set; }        
        public ReportParameter[] Parameters { get; set; }

    }
}
