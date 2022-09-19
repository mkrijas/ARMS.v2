using System.Collections.Generic;
using BoldReports.Web;

namespace Views.Data
{
    public class BoldReportViewerOptions
    {
        public string ReportName { get; set; }
        public string ServiceURL { get; set; }
        public ReportParameter[] Parameters { get; set; }

    }
}
