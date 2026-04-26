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

function IFrameHightSetter(iframeId) {
    let id = iframeId || "PreviewClaim";
    let frame = document.getElementById(id);
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


function runReport(url, iframeId) {
    var id = iframeId || "PreviewClaim";
    var iframe = document.getElementById(id);
    if (iframe) {
        iframe.src = url;
        return true;
    } else {
        console.error("Iframe with id '" + id + "' not found.");
        return false;
    }
}

// Reliable version: registers onload callback BEFORE setting src so the event
// is never missed, then calls back into Blazor via DotNetObjectReference.
// The iframe is already hidden via display:none by Blazor before this runs,
// so no stale content flash — no need for an about:blank intermediate step.
function runReportWithCallback(iframeId, url, dotNetRef) {
    if (window._ssrsLoadTimeout) clearTimeout(window._ssrsLoadTimeout);
    
    const iframe = document.getElementById(iframeId);
    if (!iframe) {
        console.error(`[runReportWithCallback] iframe not found: ${iframeId}`, dotNetRef);
        return false;
    }

    // Safely clear old timeout/handler if any
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

    // Safety timeout: if SSRS fails to respond or is too slow, hide the loader after 12s
    window._ssrsLoadTimeout = setTimeout(function() {
        if (iframe.onload) {
            console.warn("[runReportWithCallback] Load timeout reached for " + url);
            iframe.onload();
        }
    }, 120000);

    // Set src to start the load
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

/**
 * Initialise SSRS page pagination after iframe load.
 * Splits the all-pages HTML4.0 response into individually togglable divs.
 */
function initReportPages(iframeId) {
    console.log("[initReportPages] Starting pagination scan for iframe:", iframeId);
    return new Promise((resolve) => {
        let attempts = 0;
        const maxAttempts = 30;
        const check = () => {
            attempts++;
            try {
                var iframe = document.getElementById(iframeId);
                if (!iframe || !iframe.contentDocument) {
                    if (attempts < maxAttempts) { setTimeout(check, 200); return; }
                    return resolve(1);
                }
                var doc = iframe.contentDocument;

                // Wait for SSRS processing indicator to clear
                var isWaiting = doc.querySelector('[id*="WaitControl"], .WaitControl, [id*="Processing"]') !== null;
                if (isWaiting && attempts < maxAttempts) {
                    setTimeout(check, 200);
                    return;
                }

                var body = doc.body;
                var bodyText = (body ? body.innerText : "") || "";

                // If body is too short and still loading, retry
                if (bodyText.trim().length < 30 && attempts < maxAttempts) {
                    setTimeout(check, 200);
                    return;
                }

                // Error detection — only match SSRS-specific error strings, NOT generic numbers
                var errTerms = ["rsWrongItemType", "rsUnknownReportParameter", "rsReportParameterTypeMismatch", "Reporting Services Error", "The item '/"];
                if (errTerms.some(t => bodyText.includes(t))) {
                    console.warn("[initReportPages] SSRS error in content:", bodyText.substring(0, 200));
                    return resolve(1);
                }

                // Empty report detection
                if (bodyText.trim().length < 30) {
                    return resolve(0);
                }

                // === Strategy 1: SSRS 2017+ modern renderer ===
                var modernPages = Array.from(doc.querySelectorAll('.msrp-body-page'));
                if (modernPages.length > 1) {
                    console.log("[initReportPages] Strategy 1: Found", modernPages.length, "modern pages (.msrp-body-page)");
                    return resolve(_wrapAndHidePages(modernPages, iframe));
                }

                // === Strategy 2: SSRS HTML4.0 — oReportCell divs ===
                // Each page is a div/td with id containing "_oReportCell"
                var reportCells = Array.from(doc.querySelectorAll('[id*="_oReportCell"]'));
                // Filter to top-level ones only (remove nested)
                reportCells = reportCells.filter(p => !reportCells.some(other => other !== p && other.contains(p)));
                if (reportCells.length > 1) {
                    console.log("[initReportPages] Strategy 2: Found", reportCells.length, "oReportCell pages");
                    return resolve(_wrapAndHidePages(reportCells, iframe));
                }

                // === Strategy 3: page-break elements ===
                // SSRS HTML4.0 puts <br style="page-break-after:always"> between pages
                var breakSelector = [
                    'div[style*="page-break"]',
                    'br[style*="page-break"]',
                    'td[style*="page-break"]',
                    'tr[style*="page-break"]',
                    'table[style*="page-break"]',
                    'span[style*="page-break"]'
                ].join(',');
                var breaks = Array.from(doc.querySelectorAll(breakSelector));
                if (breaks.length > 0) {
                    console.log("[initReportPages] Strategy 3: Found", breaks.length, "page-break elements");
                    // _groupByBreaks returns already-wrapped divs ready for show/hide
                    var wrappers = _groupByBreaks(doc, breaks);
                    if (wrappers.length > 1) {
                        console.log("[initReportPages] Strategy 3: Created", wrappers.length, "wrapper pages from breaks");
                        return resolve(_wrapAndHidePages(wrappers, iframe));
                    }
                }

                // === Strategy 4: look for hidden input page markers ===
                var hiddenCells = Array.from(doc.querySelectorAll('input[type="hidden"][id*="ReportCell"], input[type="hidden"][id*="PageCell"]'));
                if (hiddenCells.length > 1) {
                    var parentDivs = hiddenCells.map(h => {
                        var p = h.parentNode;
                        while (p && p.tagName !== 'DIV' && p.tagName !== 'TD' && p !== body) p = p.parentNode;
                        return p;
                    }).filter((v, i, self) => v && self.indexOf(v) === i);
                    if (parentDivs.length > 1) {
                        console.log("[initReportPages] Strategy 4: Found", parentDivs.length, "hidden-cell pages");
                        return resolve(_wrapAndHidePages(parentDivs, iframe));
                    }
                }

                // === Strategy 5: top-level large children of body ===
                // This is a last-resort for reports that are flat lists of tables/divs
                var topElements = Array.from(body.children).filter(el => {
                    var tag = el.tagName;
                    return (tag === 'TABLE' || tag === 'DIV') && el.offsetHeight > 100;
                });
                if (topElements.length > 1) {
                    console.log("[initReportPages] Strategy 5: Found", topElements.length, "top-level elements as pages");
                    return resolve(_wrapAndHidePages(topElements, iframe));
                }

                // === Strategy 6: Text-based Header Detection ===
                // Search for the report title text which repeats on every page
                var allDivs = Array.from(doc.querySelectorAll('div, span, td'));
                var textBreaks = allDivs.filter(el => {
                    var text = (el.innerText || el.textContent || "").trim().toUpperCase();
                    return text.includes("CONSIGNMENT NOTE REPORT");
                }).map(el => {
                    // Walk up to the TR or top-level DIV that contains this header
                    var p = el.parentNode;
                    while (p && p.tagName !== 'TR' && p.tagName !== 'TABLE' && p !== body) {
                        if (p.parentNode === body || (p.parentNode && p.parentNode.id === 'oReportCell')) break;
                        p = p.parentNode;
                    }
                    return p || el;
                });

                // Remove duplicates (if multiple elements in same row matched)
                textBreaks = textBreaks.filter((v, i, self) => v && self.indexOf(v) === i);

                if (textBreaks.length > 1) {
                    console.log("[initReportPages] Strategy 6: Found", textBreaks.length, "repeating headers as pages");
                    var wrappers = _groupByBreaks(doc, textBreaks);
                    if (wrappers.length > 1) {
                        return resolve(_wrapAndHidePages(wrappers, iframe));
                    }
                }

                // === Fallback: 1 page (body has content) ===
                const pageBoundaries = _findSSRSPages(doc);
                if (pageBoundaries.length > 1) { // Changed from 0 to 1 to match original logic of needing >1 page
                    const pages = _groupByBreaks(doc, pageBoundaries);
                    return resolve(_wrapAndHidePages(pages, iframe)); // Pass iframe to _wrapAndHidePages
                }
                iframe._ssrsPages = [body];
                return resolve(1);

            } catch (e) {
                console.warn('[initReportPages] Exception:', e);
                return resolve(1);
            }
        };
        setTimeout(check, 300);
    });
}

/** 
 * Hide all pages except the first, store them on the iframe, return count.
 */
function _wrapAndHidePages(pages, iframe) {
    iframe._ssrsPages = pages;
    pages.forEach((p, i) => {
        if (!p._origDisplay) p._origDisplay = p.style.display || (p.tagName === 'TBODY' ? 'table-row-group' : 'block');
        p.style.display = i === 0 ? p._origDisplay : 'none';
    });
    if (iframe.contentWindow) iframe.contentWindow.scrollTo(0, 0);
    return pages.length;
}

/**
 * Show pageNum (1-based) and hide all other pages.
 */
function showReportPage(iframeId, pageNum) {
    try {
        var iframe = document.getElementById(iframeId);
        if (!iframe || !iframe._ssrsPages) return;
        var pages = iframe._ssrsPages;
        var idx = Math.max(0, Math.min(pageNum - 1, pages.length - 1));
        pages.forEach((p, i) => {
            p.style.display = i === idx ? (p._origDisplay || 'block') : 'none';
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

    // Helper: get the direct child of `ancestor` that contains (or is) `el`
    function getDirectChild(ancestor, el) {
        var node = el;
        while (node && node.parentNode !== ancestor) {
            node = node.parentNode;
            if (!node || node === doc.documentElement) return null;
        }
        return node;
    }

    // 1. Determine common container (LCA or explicit)
    var container = null;
    var reportCell = doc.getElementById('oReportCell');
    
    if (breaks.length === 1) {
        // Single break: just split its parent
        container = breaks[0].parentNode;
    } else {
        // Multi-break: Find LCA
        container = breaks[0].parentNode;
        while (container && container !== doc.documentElement) {
            var directChildren = breaks.map(b => getDirectChild(container, b)).filter(n => n);
            var unique = new Set(directChildren);
            
            // If this container has children referencing multiple breaks, it's our split point
            if (unique.size >= 2) break;
            
            // Heuristic: If we hit oReportCell or body, we MUST split here even if unique count is low
            if (container === reportCell || container === body) break;
            
            container = container.parentNode;
        }
    }

    if (!container || container === doc.documentElement) {
        console.log("[_groupByBreaks] Failed to find common container for", breaks.length, "breaks");
        return [];
    }

    console.log("[_groupByBreaks] Splitting container:", container.tagName, "ID:", container.id, "with", breaks.length, "breaks");

    // 2. Identify the divider elements (direct children of container that lead to breaks)
    var breakDividersList = breaks.map(b => getDirectChild(container, b)).filter(n => n);
    var breakDividers = new Set(breakDividersList);
    console.log("[_groupByBreaks] Unique dividers in container:", breakDividers.size);

    // 3. Group childNodes
    var children = Array.from(container.childNodes);
    var groups = [];
    var current = [];

    children.forEach(function(node) {
        // If this node is a divider, start a new group
        if (breakDividers.has(node)) {
            if (current.length > 0) {
                groups.push(current.slice());
            }
            current = [];
        }
        current.push(node);
    });
    if (current.length > 0) {
        groups.push(current.slice());
    }

    console.log("[_groupByBreaks] Produced", groups.length, "groups");

    // 4. Wrap each group in a container (div or tbody)
    return groups
        .filter(nodes => nodes.length > 0 && nodes[0] && nodes[0].parentNode)
        .map(function(nodes, gIdx) {
            var firstNode = nodes[0];
            var parent = firstNode.parentNode;
            
            // Standard SSRS: TRs should be wrapped in TBODY, everything else in DIV
            var isTableSplit = (firstNode.tagName === 'TR');
            var wrapperTag = isTableSplit ? 'tbody' : 'div';
            
            var wrapper = doc.createElement(wrapperTag);
            wrapper.className = 'ssrs-page-wrapper';
            
            if (isTableSplit) {
                wrapper.style.display = 'table-row-group'; 
                wrapper._origDisplay = 'table-row-group';
            } else {
                wrapper.style.display = 'block';
                wrapper.style.width = '100%';
                wrapper._origDisplay = 'block';
            }

            parent.insertBefore(wrapper, firstNode);
            nodes.forEach(function(n) { wrapper.appendChild(n); });
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