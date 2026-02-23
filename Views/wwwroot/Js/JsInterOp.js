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
    if (frame && frame.contentWindow && frame.contentWindow.document && frame.contentWindow.document.body) {
        let scrollHeight = frame.contentWindow.document.body.scrollHeight;
        frame.style.height = (scrollHeight ?? '3000') + 'px';
    }
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

// Reliable version: registers onload callback BEFORE setting src so the event
// is never missed, then calls back into Blazor via DotNetObjectReference.
// The iframe is already hidden via display:none by Blazor before this runs,
// so no stale content flash — no need for an about:blank intermediate step.
function runReportWithCallback(url, dotNetRef) {
    var iframe = document.getElementById("PreviewClaim");
    if (!iframe) {
        console.error("[runReportWithCallback] PreviewClaim iframe not found.");
        return false;
    }
    // Register load handler BEFORE setting src (same JS tick — event never missed)
    iframe.onload = function () {
        iframe.onload = null;   // fire once only
        if (dotNetRef) {
            dotNetRef.invokeMethodAsync("OnIframeLoadComplete")
                     .catch(function (e) { console.warn("[runReportWithCallback] callback error:", e); });
        }
    };
    iframe.src = url;
    return true;
}

function focusElement(id) {
    const element = document.getElementById(id);
    if (element) {
        element.focus();
    }
}

window.BlazorHelpers = {
    RedirectTo: function (path) {
        $.post(path);
    },
    addEnterKeyListener: function (dotNetHelper) {
        // Ensure we don't have duplicate listeners
        if (window.enterKeyListener) {
            document.removeEventListener('keydown', window.enterKeyListener);
        }

        window.enterKeyListener = function (e) {
            // Ignore if the event originated from an input, textarea, select, or button
            const tagName = e.target.tagName;
            if (tagName === 'INPUT' || tagName === 'TEXTAREA' || tagName === 'SELECT' || tagName === 'BUTTON') {
                return;
            }

            if (e.key === "Enter") {
                // Also prevent default to avoid scrolling/other side effects on body
                e.preventDefault();
                dotNetHelper.invokeMethodAsync('OnEnterKeyPressed');
            }
        };
        document.addEventListener('keydown', window.enterKeyListener);
    },
    removeEnterKeyListener: function () {
        if (window.enterKeyListener) {
            document.removeEventListener('keydown', window.enterKeyListener);
            window.enterKeyListener = null; // Clear the reference
        }
    }
};


// let element = document.getElementById("treeviewitem")
// console.log(element);


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
    try {
        if (link.parentNode) {
            link.parentNode.removeChild(link);
        }
    } catch (e) {
        console.warn("Retrying removing child after download failed:", e);
    }
}


function resetHorizontalScrollbarForElement(element) {
    if (element) {
        element.scrollLeft = 0;
    }
}

// ─── Custom Report Toolbar Helpers ────────────────────────────────────────────

/**
 * Initialise SSRS page pagination after iframe load.
 * Finds all page containers, hides pages 2..N, returns total page count.
 * Works across SSRS 2016+ (msrp-body-page), SSRS 2014 (_oReportCell IDs),
 * and older HTML4.0 renderers (page-break separators).
 */
function initReportPages(iframeId) {
    try {
        var iframe = document.getElementById(iframeId);
        if (!iframe || !iframe.contentDocument) return 1;
        var doc = iframe.contentDocument;

        var pages = _findSSRSPages(doc);
        if (pages.length < 2) {
            // Single page — nothing to hide; cache length 1 so "of 1" shows
            iframe._ssrsPages = pages.length === 1 ? pages : null;
            return pages.length || 1;
        }

        // Cache on the element; save original display so we can restore it
        iframe._ssrsPages = pages;
        pages.forEach(function (p, i) {
            p._ssrsOrigDisplay = p.style.display || '';
            p.style.display = i === 0 ? p._ssrsOrigDisplay : 'none';
        });

        // Scroll to top of report
        iframe.contentWindow.scrollTo(0, 0);
        return pages.length;
    } catch (e) {
        console.warn('[initReportPages]', e);
        return 1;
    }
}

/**
 * Show pageNum (1-based) and hide all other pages.
 * Must be called AFTER initReportPages.
 */
function showReportPage(iframeId, pageNum) {
    try {
        var iframe = document.getElementById(iframeId);
        if (!iframe || !iframe._ssrsPages) return;

        var pages = iframe._ssrsPages;
        var idx = Math.max(0, Math.min(pageNum - 1, pages.length - 1));
        pages.forEach(function (p, i) {
            p.style.display = i === idx ? p._ssrsOrigDisplay : 'none';
        });

        if (iframe.contentWindow) iframe.contentWindow.scrollTo(0, 0);
    } catch (e) {
        console.warn('[showReportPage]', e);
    }
}

/** Internal: try multiple selectors to locate SSRS page elements */
function _findSSRSPages(doc) {
    var pages;

    // SSRS 2017+ new HTML renderer: class="msrp-body-page"
    pages = Array.from(doc.querySelectorAll('.msrp-body-page'));
    if (pages.length > 0) return pages;

    // SSRS 2016 HTML4.0: id ends with "_oReportCell"
    pages = Array.from(doc.querySelectorAll('[id$="_oReportCell"]'));
    if (pages.length > 0) return pages;

    // Older SSRS: pages separated by <br style="page-break-after:always">
    // or <div style="page-break-after:always">. Wrap the sibling groups.
    var breaks = Array.from(doc.querySelectorAll(
        'br[style*="page-break-after"], div[style*="page-break-after"], td[style*="page-break-after"]'
    ));
    if (breaks.length > 0) {
        return _groupByBreaks(doc, breaks);
    }

    return [];
}

/** Wrap content around page-break markers into synthetic page divs */
function _groupByBreaks(doc, breaks) {
    var container = breaks[0].parentNode;
    if (!container) return [];

    var children = Array.from(container.childNodes);
    var groups = [];
    var current = [];

    children.forEach(function (node) {
        var isBreak = breaks.indexOf(node) >= 0;
        if (isBreak) {
            if (current.length > 0) groups.push(current);
            current = [];
        } else {
            current.push(node);
        }
    });
    if (current.length > 0) groups.push(current);

    // Hide the break markers themselves so they don't show as blank lines
    breaks.forEach(function (b) { b.style.display = 'none'; });

    // Wrap each group in a <div> so we can toggle display
    return groups.map(function (nodes) {
        var wrapper = doc.createElement('div');
        wrapper.style.cssText = 'display:block';
        nodes[0].parentNode.insertBefore(wrapper, nodes[0]);
        nodes.forEach(function (n) { wrapper.appendChild(n); });
        return wrapper;
    });
}

/**
 * Trigger the browser Print dialog for the content inside the named iframe.
 */
function printIframe(iframeId) {
    try {
        var iframe = document.getElementById(iframeId);
        if (iframe && iframe.contentWindow) {
            iframe.contentWindow.focus();
            iframe.contentWindow.print();
        }
    } catch (e) {
        console.warn('[printIframe]', e);
    }
}

/**
 * Find (and highlight) text inside the iframe using the browser's native
 * find API. Pass findNext=true to advance to the subsequent match.
 */
function findInIframe(iframeId, text, findNext) {
    try {
        var iframe = document.getElementById(iframeId);
        if (!iframe || !iframe.contentWindow) return;
        var win = iframe.contentWindow;

        // Modern browsers expose window.find()
        if (typeof win.find === 'function') {
            win.focus();
            // Reset to top on a new search term; advance on findNext
            win.find(text, false /*case*/, findNext /*backwards=false=>forward*/, true /*wrap*/);
        }
    } catch (e) {
        console.warn('[findInIframe]', e);
    }
}