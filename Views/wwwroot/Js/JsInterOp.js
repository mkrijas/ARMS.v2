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

    // Safely clear old timeout if any
    if (window._ssrsLoadTimeout) clearTimeout(window._ssrsLoadTimeout);

    // Register load handler BEFORE setting src
    iframe.onload = function () {
        if (window._ssrsLoadTimeout) clearTimeout(window._ssrsLoadTimeout);
        iframe.onload = null;
        if (dotNetRef) {
            dotNetRef.invokeMethodAsync("OnIframeLoadComplete")
                     .catch(function (e) { console.warn("[runReportWithCallback] callback error:", e); });
        }
    };

    // Safety timeout: if SSRS fails to respond or is too slow, hide the loader after 15s
    window._ssrsLoadTimeout = setTimeout(function() {
        if (iframe.onload) {
            console.warn("[runReportWithCallback] Load timeout reached for " + url);
            iframe.onload();
        }
    }, 15000);

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

    // 1. Direct Page Containers (SSRS 2017+ new HTML renderer)
    pages = Array.from(doc.querySelectorAll('.msrp-body-page'));
    if (pages.length > 0) return pages;

    // 2. Report Cells as Page Containers (SSRS 2016- HTML4.0)
    // Sometimes prefixed, so we check if ID contains _oReportCell
    pages = Array.from(doc.querySelectorAll('[id*="_oReportCell"]'));
    if (pages.length > 0) {
        // Filter out nested ones if any (rare but possible)
        return pages.filter(p => !pages.some(other => other !== p && other.contains(p)));
    }

    // 3. Page Break Markers (Separators)
    // We look for page-break-after or page-break-before in style attributes
    var breakSelector = [
        'br[style*="page-break" i]',
        'div[style*="page-break" i]',
        'td[style*="page-break" i]',
        'tr[style*="page-break" i]',
        'span[style*="page-break" i]'
    ].join(',');
    
    var breaks = Array.from(doc.querySelectorAll(breakSelector));
    if (breaks.length > 0) {
        return _groupByBreaks(doc, breaks);
    }

    // 4. Hidden Input Markers
    var hiddenCells = Array.from(doc.querySelectorAll('input[type="hidden"][id*="_oReportCell"]'));
    if (hiddenCells.length > 0) {
        return _groupByBreaks(doc, hiddenCells);
    }

    return [];
}

/** Wrap content around page-break markers into synthetic page divs */
function _groupByBreaks(doc, breaks) {
    if (!breaks || breaks.length === 0) return [];

    // Find the common container. Usually they are siblings in a main div/body.
    var firstBreak = breaks[0];
    var container = firstBreak.parentNode;
    
    // Walk up to find the container that holds the bulk of the report
    while (container && container.childNodes.length <= 1 && container.tagName !== 'BODY') {
        container = container.parentNode;
    }
    if (!container) return [];

    var children = Array.from(container.childNodes);
    var groups = [];
    var current = [];

    children.forEach(function (node) {
        current.push(node);
        // Check if node is a break OR contains a break element
        var isBreak = breaks.indexOf(node) >= 0 || 
                      (node.querySelector && breaks.some(function(b) { return node.contains(b); }));
        
        if (isBreak) {
            if (current.length > 0) groups.push(current);
            current = [];
        }
    });
    if (current.length > 0) groups.push(current);

    // Hide the break markers so they don't add extra whitespace
    breaks.forEach(function (b) { 
        try { if (b.style) b.style.display = 'none'; } catch(e){}
    });

    // Wrap each group in a <div> so we can toggle visibility
    return groups.map(function (nodes) {
        var wrapper = doc.createElement('div');
        wrapper.className = 'ssrs-page-wrapper';
        wrapper.style.cssText = 'display:block; width:100%;';
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

/**
 * Scroll to the first element matching a given class name.
 */
function scrollToClass(className) {
    setTimeout(function() {
        var element = document.querySelector('.' + className);
        if (element) {
            element.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }
    }, 100);
}

// --- Sound Effects (Web Audio API) ---

function _createWhoosh(ctx, startTime, duration, startFreq, endFreq, volume) {
    if (volume === undefined) volume = 0.2;
    
    var bufferSize = ctx.sampleRate * duration;
    var buffer = ctx.createBuffer(1, bufferSize, ctx.sampleRate);
    var data = buffer.getChannelData(0);
    for (var i = 0; i < bufferSize; i++) {
        data[i] = Math.random() * 2 - 1; 
    }
    
    var noise = ctx.createBufferSource();
    noise.buffer = buffer;
    
    var filter = ctx.createBiquadFilter();
    filter.type = 'bandpass';
    filter.Q.value = 1;
    filter.frequency.setValueAtTime(startFreq, startTime);
    filter.frequency.exponentialRampToValueAtTime(endFreq, startTime + duration);
    
    var gain = ctx.createGain();
    gain.gain.setValueAtTime(0, startTime);
    gain.gain.linearRampToValueAtTime(volume, startTime + duration * 0.3); // Peak volume
    gain.gain.linearRampToValueAtTime(0, startTime + duration);
    
    noise.connect(filter);
    filter.connect(gain);
    gain.connect(ctx.destination);
    
    noise.start(startTime);
    noise.stop(startTime + duration);
}

function _createSlam(ctx, startTime, volume) {
    if (volume === undefined) volume = 1.0;
    
    // Low thud
    var osc = ctx.createOscillator();
    var gain = ctx.createGain();
    osc.type = 'sine';
    osc.frequency.setValueAtTime(120, startTime);
    osc.frequency.exponentialRampToValueAtTime(20, startTime + 0.25);
    
    gain.gain.setValueAtTime(volume * 0.8, startTime);
    gain.gain.exponentialRampToValueAtTime(0.001, startTime + 0.25);
    
    osc.connect(gain);
    gain.connect(ctx.destination);
    
    osc.start(startTime);
    osc.stop(startTime + 0.25);

    // High impact crack
    var osc2 = ctx.createOscillator();
    var gain2 = ctx.createGain();
    osc2.type = 'triangle';
    osc2.frequency.setValueAtTime(600, startTime);
    osc2.frequency.exponentialRampToValueAtTime(100, startTime + 0.04);
    
    gain2.gain.setValueAtTime(volume * 0.4, startTime);
    gain2.gain.exponentialRampToValueAtTime(0.001, startTime + 0.04);
    
    osc2.connect(gain2);
    gain2.connect(ctx.destination);
    
    osc2.start(startTime);
    osc2.stop(startTime + 0.04);
}

function _createSnap(ctx, startTime) {
    var bufferSize = ctx.sampleRate * 0.05; // 50ms Noise burst
    var buffer = ctx.createBuffer(1, bufferSize, ctx.sampleRate);
    var data = buffer.getChannelData(0);
    for (var i = 0; i < bufferSize; i++) {
        data[i] = Math.random() * 2 - 1; 
    }
    
    var noise = ctx.createBufferSource();
    noise.buffer = buffer;
    
    var filter = ctx.createBiquadFilter();
    filter.type = 'highpass';
    filter.frequency.setValueAtTime(2500, startTime); 
    
    var gain = ctx.createGain();
    gain.gain.setValueAtTime(0, startTime);
    // Instant volume spike, fast decay
    gain.gain.linearRampToValueAtTime(0.3, startTime + 0.003); 
    gain.gain.exponentialRampToValueAtTime(0.001, startTime + 0.05); 
    
    noise.connect(filter);
    filter.connect(gain);
    gain.connect(ctx.destination);
    
    noise.start(startTime);
    noise.stop(startTime + 0.05);
}

window.SoundEffects = {
    playLoginEntry: function() {
        try {
            var ctx = new (window.AudioContext || window.webkitAudioContext)();
            if (ctx.state === 'suspended') {
                ctx.resume();
            }
            var now = ctx.currentTime;
            
            // 1. Whoosh drop (0s - 0.4s)
            _createWhoosh(ctx, now, 0.4, 150, 800); 
            
            // 2. Main Impact at 0.4s (dropDown keyframe)
            _createSlam(ctx, now + 0.4, 1.0);
            
            // 3. Second bounce at 0.55s
            _createSlam(ctx, now + 0.55, 0.4);
            
            // 4. Third bounce at 0.7s
            _createSlam(ctx, now + 0.7, 0.2);
            
            console.log("[SoundEffects] Entry sound triggered");
        } catch (e) {
            console.warn("[SoundEffects] Failed to play entry sound:", e);
        }
    },
    playLoginExit: function() {
        try {
            var ctx = new (window.AudioContext || window.webkitAudioContext)();
            if (ctx.state === 'suspended') {
                ctx.resume();
            }
            var now = ctx.currentTime;
            
            // 1. Smooth Air Swoosh (duration 0.8s, soft lowpass filter)
            var bufferSize = ctx.sampleRate * 0.8; 
            var buffer = ctx.createBuffer(1, bufferSize, ctx.sampleRate);
            var data = buffer.getChannelData(0);
            for (var i = 0; i < bufferSize; i++) {
                data[i] = Math.random() * 2 - 1; 
            }
            
            var noise = ctx.createBufferSource();
            noise.buffer = buffer;
            
            var filter = ctx.createBiquadFilter();
            filter.type = 'lowpass';
            filter.frequency.setValueAtTime(800, now); 
            filter.frequency.exponentialRampToValueAtTime(100, now + 0.8);
            
            var gain = ctx.createGain();
            gain.gain.setValueAtTime(0, now);
            gain.gain.linearRampToValueAtTime(0.12, now + 0.2); // Smooth peak
            gain.gain.linearRampToValueAtTime(0, now + 0.8); // Smooth fall off
            
            noise.connect(filter);
            filter.connect(gain);
            gain.connect(ctx.destination);
            
            noise.start(now);
            noise.stop(now + 0.8);
            
            console.log("[SoundEffects] Exit air-swoosh triggered");
        } catch (e) {
            console.warn("[SoundEffects] Failed to play exit sound:", e);
        }
    }
};