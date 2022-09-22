window.BoldReports = {
    RenderViewer: function (elementID, reportViewerOptions) {
        $("#" + elementID).boldReportViewer({
            reportPath: reportViewerOptions.reportPath,
            reportServiceUrl: reportViewerOptions.serviceURL,
            reportServerUrl: reportViewerOptions.serverURL,
            serviceAuthorizationToken: reportViewerOptions.serviceAuthorizationToken,
            Parameters: reportViewerOptions.Parameters
        });
    }
}