window.triggerFileDownload = function (fileName, fileData) {
    // Convert Base64 string to a Blob
    var byteCharacters = atob(fileData);
    var byteNumbers = new Array(byteCharacters.length);
    for (var i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    var byteArray = new Uint8Array(byteNumbers);
    var blob = new Blob([byteArray], { type: 'application/octet-stream' });

    // Create a URL for the Blob
    var url = URL.createObjectURL(blob);
    downloadfile(url, fileName);
    URL.revokeObjectURL(url);
}

window.downloadfile = function (url, filename) {
    // Create an anchor element for downloading
    var anchor = document.createElement('a');
    anchor.href = url;
    anchor.download = filename;

    // Programmatically click the anchor to initiate download
    anchor.click();

    // Release the object URL
    anchor.remove();
}

//function DownloadFile() {
//    var downloadLink = document.getElementById('downloadLink');
//    downloadLink.click();
//}