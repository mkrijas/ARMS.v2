window.BoldReports = {
    RenderViewer: function (elementID, reportViewerOptions) {
        $("#" + elementID).boldReportViewer({
            reportPath: reportViewerOptions.reportPath,
            reportServiceUrl: reportViewerOptions.serviceURL, 
            parameters: reportViewerOptions.parameters
        });
    }
}