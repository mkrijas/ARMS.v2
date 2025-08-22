var _0x1751 = ["arms@teamthai.in", "arms@123"];

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

function IFrameHightSetter() {
    let frame = document.getElementById("PreviewClaim");
    let scrollHeight = frame?.contentWindow?.document?.body?.scrollHeight;
    frame.style.height = (scrollHeight ?? '3000') + 'px';
    return false;

}

//function runReport(url) {
//    var http = getHTTPObject();
//    http.onreadystatechange = function () {
//        if (http.readyState == 4) {
//            if (http.status == 401) {
//                runReport();
//            }
//            if (http.status == 200) {
//                document.location = url;
//            }
//        }
//    };
//    http.open("get", url, true);

//    http.send(null);
//    return false;
//}


function runReport(url) {
    var iframe = document.getElementById("PreviewClaim");
    if (iframe) {
        iframe.src = url;
        return true;
    } else {
        console.error("Iframe with id 'PreviewClaim' not found.");
        return false;
    }
}

function focusElement(id) {
    const element = document.getElementById(id);
    element.focus();
}


window.BlazorHelpers = {
    RedirectTo: function (path) {
        $.post(path);
    }
};


let element = document.getElementById("treeviewitem")
console.log(element);


async function getElementCoordinates(item, targetBox) {
    var element = document.getElementById(item);
    var targetElement = document.getElementById(targetBox);

    if (element) {
        var rect = element.getBoundingClientRect();
        var scrollTop = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop || 0;
        targetElement.style.left = 600 + "px";
        targetElement.style.top = (((rect.top - 245) < 96) ? (scrollTop + 96) : ((rect.top - 245) + scrollTop)) + "px";
        return {
            x: rect.left,
            y: rect.top,
        };
    }
    return null;
};


function saveAsFile(filename, bytesBase64) {
    var link = document.createElement('a');
    link.download = filename;
    link.href = "data:application/octet-stream;base64," + bytesBase64;
    document.body.appendChild(link); // Needed for Firefox
    link.click();
    document.body.removeChild(link);
}


function resetHorizontalScrollbarForElement(element) {
    if (element) {
        element.scrollLeft = 0;
    }
}