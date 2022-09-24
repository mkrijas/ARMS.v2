using System.Collections.Generic;
using BoldReports.Web;

namespace Views.Data
{
    public class BoldReportViewerOptions
    {
        public string ReportPath { get; set; }
        public string ServiceURL { get; set; }
        public List<ReportParameter> Parameters { get; set; }

    }
}
