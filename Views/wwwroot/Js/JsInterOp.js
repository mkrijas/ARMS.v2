var _0x1751 = ["RIJASMK", "Ri*tt012"];

function getHTTPObject() {
    if (typeof XMLHttpRequest != 'undefined') {
        return new XMLHttpRequest();
    }
    try {
        return new ActiveXObject("Msxml2.XMLHTTP");
    } catch (e) {
        try {
            return new ActiveXObject("Microsoft.XMLHTTP");
        } catch (e) { }
    }
    return false;
}



function runReport(url) {
    var http = getHTTPObject();
    /*var url = "http://rs-01/ReportServer/Pages/ReportViewer.aspx?%2fTEL+Wall+Thing%2fWallDash&rs:Command=Render";*/
    http.onreadystatechange = function () {
        if (http.readyState == 4) {
            if (http.status == 401) {
                runReport();
            }
            if (http.status == 200) {
                document.location = url;
            }
        }
    };
    http.open("get", url, true, _0x1751[0x0], _0x1751[0x1]);
    http.send(null);
    return false;
}


function focusElement (id) {
    const element = document.getElementById(id);
    element.focus();
}