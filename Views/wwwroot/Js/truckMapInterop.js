window.truckMapInterop = {
    mapsData: {},
    locationCache: {},
    pendingGeocodes: {},

    getAddress: async function (lat, lng) {
        const latLngKey = `${lat.toFixed(4)},${lng.toFixed(4)}`;
        if (this.locationCache[latLngKey]) {
            return this.locationCache[latLngKey];
        }
        if (this.pendingGeocodes[latLngKey]) {
            return await this.pendingGeocodes[latLngKey];
        }

        const url = `https://maps.googleapis.com/maps/api/geocode/json?latlng=${lat},${lng}&key=AIzaSyCvBqso9Nzx2cIcZsnG4zb24fkgLwSHmys`;
        const promise = fetch(url)
            .then(res => res.json())
            .then(data => {
                if (data.status === "OK" && data.results.length > 0) {
                    let address = data.results[0].formatted_address;
                    this.locationCache[latLngKey] = address;
                    return address;
                }
                return "Unknown Location";
            })
            .catch(e => "Location Fetch Error");

        this.pendingGeocodes[latLngKey] = promise;
        return promise;
    },

    buildCustomHtml: function (speed, gear, rpm, fuel, def, alt, odo, date, regn, popupSvg, popupBorder, locStr) {
        const cleanRegn = regn.replace(/\W/g, '');
        return `<div style="font-family: 'Roboto', sans-serif; width: 440px; padding: 22px; box-sizing: border-box; position: relative;">
                    <button class="close-popup-btn" style="position: absolute; top: 16px; right: 16px; background: #f0f0f0; border: none; font-size: 20px; color: #555; cursor: pointer; width: 30px; height: 30px; border-radius: 50%; display: flex; align-items: center; justify-content: center; transition: all 0.2s; z-index: 2;">&times;</button>
                    <div style="display: flex; align-items: center; justify-content: flex-start; border-bottom: ${popupBorder}; padding-bottom: 12px; margin-bottom: 14px; padding-right: 32px;">
                        ${popupSvg}
                        <h3 style="margin: 0; color: #333; font-size: 21px; font-weight: 700; line-height: 1;">${regn}</h3>
                    </div>
                    <div style="margin-bottom: 14px; background: #f4f6f8; padding: 10px 12px; border-radius: 8px; border: 1px solid #e1e4e8; display: flex; align-items: flex-start; gap: 8px;">
                        <svg style="min-width: 16px; width: 16px; height: 16px; margin-top: 2px;" fill="#D32F2F" viewBox="0 0 24 24"><path d="M12 2C8.13 2 5 5.13 5 9c0 5.25 7 13 7 13s7-7.75 7-13c0-3.87-3.13-7-7-7zm0 9.5c-1.38 0-2.5-1.12-2.5-2.5s1.12-2.5 2.5-2.5 2.5 1.12 2.5 2.5-1.12 2.5-2.5 2.5z"/></svg>
                        <span style="font-size: 13px; font-weight: 500; color: #444; line-height: 1.4;" class="loc-text-${cleanRegn}">${locStr}</span>
                    </div>
                    <div style="display: grid; grid-template-columns: auto 1fr auto 1fr; gap: 10px 15px; font-size: 13px; color: #444;">
                        <strong style="color: #555;">Speed:</strong> <span style="font-weight: 600; color: ${speed > 0 ? '#2E7D32' : '#333'}">${Math.round(speed)} km/h</span>
                        <strong style="color: #555;">Gear:</strong> <span style="font-weight: 500;">${gear}</span>
                        <strong style="color: #555;">Engine:</strong> <span style="font-weight: 500;">${rpm} RPM</span>
                        <strong style="color: #555;">Fuel Lvl:</strong> <span style="font-weight: 500;">${fuel} %</span>
                        <strong style="color: #555;">DEF Lvl:</strong> <span style="font-weight: 500;">${def} %</span>
                        <strong style="color: #555;">Altitude:</strong> <span style="font-weight: 500;">${alt} m</span>
                        <strong style="color: #555;">Odo:</strong> <span style="font-weight: 500;">${odo} km</span>
                        <strong style="color: #555;">Update:</strong> <span style="font-size: 12px; font-weight: 500; color: #888;">${date}</span>
                    </div>
                </div>`;
    },

    setupPopupClass: function() {
        if (window.TruckPopup) return;
        window.TruckPopup = class TruckPopup extends google.maps.OverlayView {
            constructor(position, contentHTML, map) {
                super();
                this.position = position;
                this.container = document.createElement("div");
                this.container.style.position = "absolute";
                this.container.style.transform = "translate(-50%, calc(-100% - 25px))";
                this.container.style.background = "rgba(255, 255, 255, 0.98)";
                this.container.style.backdropFilter = "blur(12px)";
                this.container.style.borderRadius = "16px";
                this.container.style.boxShadow = "0 15px 40px rgba(0,0,0,0.2)";
                this.container.style.border = "1px solid rgba(0,0,0,0.08)";
                this.container.style.padding = "0";
                this.container.style.zIndex = "1000";
                this.container.style.cursor = "default";
                
                const arrow = document.createElement("div");
                arrow.style.position = "absolute";
                arrow.style.bottom = "-10px";
                arrow.style.left = "50%";
                arrow.style.transform = "translateX(-50%)";
                arrow.style.borderLeft = "12px solid transparent";
                arrow.style.borderRight = "12px solid transparent";
                arrow.style.borderTop = "12px solid rgba(255, 255, 255, 0.98)";
                this.container.appendChild(arrow);

                const contentDiv = document.createElement("div");
                contentDiv.innerHTML = contentHTML;
                this.container.appendChild(contentDiv);
                
                let closeBtn = this.container.querySelector('.close-popup-btn');
                if(closeBtn) {
                    closeBtn.addEventListener('click', (e) => {
                        this.setMap(null);
                        e.stopPropagation();
                    });
                    closeBtn.addEventListener('mouseover', () => closeBtn.style.background = '#e0e0e0');
                    closeBtn.addEventListener('mouseout', () => closeBtn.style.background = '#f0f0f0');
                }
                this.setMap(map);
            }
            onAdd() {
                const panes = this.getPanes();
                panes.floatPane.appendChild(this.container);
                this.container.addEventListener('click', e => e.stopPropagation());
                this.container.addEventListener('wheel', e => e.stopPropagation());
            }
            draw() {
                const overlayProjection = this.getProjection();
                const pos = overlayProjection.fromLatLngToDivPixel(this.position);
                if (pos) {
                    this.container.style.left = pos.x + 'px';
                    this.container.style.top = pos.y + 'px';
                }
            }
            onRemove() {
                if (this.container.parentNode) {
                    this.container.parentNode.removeChild(this.container);
                }
            }
            close() {
                this.setMap(null);
            }
        };
    },

    initializeMap: function (elementId, trucks) {
        this.setupPopupClass();
        if (!trucks || trucks.length === 0) {
            console.warn("No trucks data available to render the map.");
            return;
        }

        const mapContainer = document.getElementById(elementId);
        if (!mapContainer) {
            console.error("Map container not found: " + elementId);
            return;
        }

        const validTrucks = trucks.filter(t => {
            const lat = Number(t.LATITUDE || t.latitude);
            const lng = Number(t.LONGITUDE || t.longitude);
            return !isNaN(lat) && !isNaN(lng) && (lat !== 0 || lng !== 0);
        });

        if (validTrucks.length === 0) {
            console.warn("No valid truck coordinates available.");
            return;
        }

        const bounds = new google.maps.LatLngBounds();
        const mapOptions = {
            zoom: 6,
            center: { lat: Number(validTrucks[0].LATITUDE || validTrucks[0].latitude), lng: Number(validTrucks[0].LONGITUDE || validTrucks[0].longitude) },
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            mapTypeControl: false,
            streetViewControl: false,
            fullscreenControl: true
        };

        const map = new google.maps.Map(mapContainer, mapOptions);

        const markers = [];

        validTrucks.forEach(truck => {
            const position = { lat: Number(truck.LATITUDE || truck.latitude), lng: Number(truck.LONGITUDE || truck.longitude) };
            
            const gear = truck.GEAR_NUM ?? 'N/A';
            const rpm = truck.ENGINE_SPEED ?? '0';
            const fuel = truck.FUEL_LEVEL ?? '0';
            const def = truck.DEF_LEVEL ?? '0';
            const alt = truck.ALTITUDE ?? '0';
            const speed = truck.SPEED ?? 0;
            const regn = truck.REGN_NUMBER ?? 'Unknown';
            const odo = truck.VEHICLE_ODO ?? 0;

            // Dynamically assign icon SVG based on truck speed
            let lorrySvg = '';
            let popupSvg = '';
            let popupBorder = '';
            if (speed > 0) {
                // Green moving truck SVG
                lorrySvg = '<svg viewBox="0 0 64 32" xmlns="http://www.w3.org/2000/svg"><rect x="4" y="4" width="38" height="20" rx="3" fill="#43A047"/><path d="M 44 10 L 52 10 C 54 10 56 12 57 14 L 60 19 C 60.5 20 60.5 21 60 22 L 59 24 H 44 Z" fill="#2E7D32"/><path d="M 46 12 L 51 12 C 52 12 53 14 54 14 L 56 18 H 46 Z" fill="#E8F5E9"/><rect x="4" y="23" width="55" height="2" fill="#555"/><circle cx="12" cy="24" r="5" fill="#212121"/><circle cx="24" cy="24" r="5" fill="#212121"/><circle cx="50" cy="24" r="5" fill="#212121"/><circle cx="12" cy="24" r="2" fill="#E0E0E0"/><circle cx="24" cy="24" r="2" fill="#E0E0E0"/><circle cx="50" cy="24" r="2" fill="#E0E0E0"/></svg>';
                popupSvg = '<svg style="margin-right: 12px; width: 44px; height: 22px;" viewBox="0 0 64 32" xmlns="http://www.w3.org/2000/svg"><rect x="4" y="4" width="38" height="20" rx="3" fill="#43A047"/><path d="M 44 10 L 52 10 C 54 10 56 12 57 14 L 60 19 C 60.5 20 60.5 21 60 22 L 59 24 H 44 Z" fill="#2E7D32"/><path d="M 46 12 L 51 12 C 52 12 53 14 54 14 L 56 18 H 46 Z" fill="#E8F5E9"/><rect x="4" y="23" width="55" height="2" fill="#555"/><circle cx="12" cy="24" r="5" fill="#212121"/><circle cx="24" cy="24" r="5" fill="#212121"/><circle cx="50" cy="24" r="5" fill="#212121"/><circle cx="12" cy="24" r="2" fill="#E0E0E0"/><circle cx="24" cy="24" r="2" fill="#E0E0E0"/><circle cx="50" cy="24" r="2" fill="#E0E0E0"/></svg>';
                popupBorder = '2px solid #43A047';
            } else {
                // Red stopped truck SVG
                lorrySvg = '<svg viewBox="0 0 64 32" xmlns="http://www.w3.org/2000/svg"><rect x="4" y="4" width="38" height="20" rx="3" fill="#E53935"/><path d="M 44 10 L 52 10 C 54 10 56 12 57 14 L 60 19 C 60.5 20 60.5 21 60 22 L 59 24 H 44 Z" fill="#C62828"/><path d="M 46 12 L 51 12 C 52 12 53 14 54 14 L 56 18 H 46 Z" fill="#E3F2FD"/><rect x="4" y="23" width="55" height="2" fill="#555"/><circle cx="12" cy="24" r="5" fill="#212121"/><circle cx="24" cy="24" r="5" fill="#212121"/><circle cx="50" cy="24" r="5" fill="#212121"/><circle cx="12" cy="24" r="2" fill="#E0E0E0"/><circle cx="24" cy="24" r="2" fill="#E0E0E0"/><circle cx="50" cy="24" r="2" fill="#E0E0E0"/></svg>';
                popupSvg = '<svg style="margin-right: 12px; width: 44px; height: 22px;" viewBox="0 0 64 32" xmlns="http://www.w3.org/2000/svg"><rect x="4" y="4" width="38" height="20" rx="3" fill="#E53935"/><path d="M 44 10 L 52 10 C 54 10 56 12 57 14 L 60 19 C 60.5 20 60.5 21 60 22 L 59 24 H 44 Z" fill="#C62828"/><path d="M 46 12 L 51 12 C 52 12 53 14 54 14 L 56 18 H 46 Z" fill="#E3F2FD"/><rect x="4" y="23" width="55" height="2" fill="#555"/><circle cx="12" cy="24" r="5" fill="#212121"/><circle cx="24" cy="24" r="5" fill="#212121"/><circle cx="50" cy="24" r="5" fill="#212121"/><circle cx="12" cy="24" r="2" fill="#E0E0E0"/><circle cx="24" cy="24" r="2" fill="#E0E0E0"/><circle cx="50" cy="24" r="2" fill="#E0E0E0"/></svg>';
                popupBorder = '2px solid #E53935';
            }
            
            const marker = new google.maps.Marker({
                position: position,
                map: map,
                title: regn,
                optimized: false, // Prevents label and icon from interleaving incorrectly
                zIndex: 1,
                icon: {
                    url: 'data:image/svg+xml;charset=UTF-8,' + encodeURIComponent(lorrySvg),
                    scaledSize: new google.maps.Size(64, 32),
                    anchor: new google.maps.Point(32, 16),
                    labelOrigin: new google.maps.Point(32, 48) // Push label clearly below the 32px height marker
                },
                label: {
                    text: regn,
                    className: speed > 0 ? "truck-marker-label truck-marker-moving" : "truck-marker-label truck-marker-stopped",
                    color: speed > 0 ? "#2E7D32" : "#D32F2F"
                }
            });

            const dateStr = truck.DATE_TIME ? new Date(truck.DATE_TIME).toLocaleString() : 'N/A';
            marker.customHtml = this.buildCustomHtml(speed, gear, rpm, fuel, def, alt, odo, dateStr, regn, popupSvg, popupBorder, 'Fetching location...');
            
            const cleanRegn = regn.replace(/\W/g, '');
            this.getAddress(position.lat, position.lng).then(loc => {
                marker.customHtml = this.buildCustomHtml(speed, gear, rpm, fuel, def, alt, odo, dateStr, regn, popupSvg, popupBorder, loc);
                const locElements = document.getElementsByClassName(`loc-text-${cleanRegn}`);
                for (let i = 0; i < locElements.length; i++) {
                    locElements[i].innerText = loc;
                }
            });

            marker.addListener('click', () => {
                const mapData = window.truckMapInterop.mapsData[elementId];
                if (mapData.activePopup) mapData.activePopup.close();
                mapData.activePopup = new window.TruckPopup(marker.getPosition(), marker.customHtml, map);
            });

            markers.push(marker);
            bounds.extend(position);
        });

        // Fit map bounds to show all truck markers
        map.fitBounds(bounds);

        // Store map instance and markers for later interaction
        this.mapsData[elementId] = {
            map: map,
            markers: markers,
            activePopup: null,
            currentIndex: -1
        };
    },

    focusNext: function (elementId) {
        const data = this.mapsData[elementId];
        if (!data || data.markers.length === 0) return null;

        // Iterate circularly 
        data.currentIndex = (data.currentIndex + 1) % data.markers.length;
        
        const marker = data.markers[data.currentIndex];

        // Replace popup
        if (data.activePopup) data.activePopup.close();
        data.activePopup = new window.TruckPopup(marker.getPosition(), marker.customHtml, data.map);

        // Stop animations for all
        data.markers.forEach(m => {
            m.setAnimation(null);
            m.setZIndex(1);
        });
        
        // Bounce the focused marker
        marker.setZIndex(9999);
        marker.setAnimation(google.maps.Animation.BOUNCE);
        setTimeout(() => marker.setAnimation(null), 2100); // 3 bounces

        // Pan to marker smoothly and zoom in
        data.map.panTo(marker.getPosition());
        data.map.setZoom(15);

        return marker.getTitle();
    },

    focusTruckByReg: function (elementId, regNumber) {
        const data = this.mapsData[elementId];
        if (!data || data.markers.length === 0 || !regNumber) return null;

        const index = data.markers.findIndex(m => m.getTitle() && m.getTitle().toLowerCase() === regNumber.toLowerCase());

        if (index === -1) return null; // Not found

        data.currentIndex = index;
        const marker = data.markers[index];

        // Replace popup
        if (data.activePopup) data.activePopup.close();
        data.activePopup = new window.TruckPopup(marker.getPosition(), marker.customHtml, data.map);

        // Stop animations for all
        data.markers.forEach(m => {
            m.setAnimation(null);
            m.setZIndex(1);
        });
        
        // Bounce the focused marker
        marker.setZIndex(9999);
        marker.setAnimation(google.maps.Animation.BOUNCE);
        setTimeout(() => marker.setAnimation(null), 2100);

        // Pan to marker smoothly and zoom in
        data.map.panTo(marker.getPosition());
        data.map.setZoom(15);

        return marker.getTitle();
    },

    updateMarkers: function (elementId, updatedTrucks) {
        console.log(updatedTrucks);
        const data = this.mapsData[elementId];
        if (!data || !data.markers || !updatedTrucks) return;

        updatedTrucks.forEach(truck => {
            // Find existing marker
            const marker = data.markers.find(m => m.getTitle() === (truck.REGN_NUMBER || truck.regN_NUMBER || truck.regn_NUMBER));
            if (marker) {
                const lat = Number(truck.LATITUDE || truck.latitude);
                const lng = Number(truck.LONGITUDE || truck.longitude);
                if (isNaN(lat) || isNaN(lng) || (lat === 0 && lng === 0)) return;

                const newPos = { lat, lng };
                // Smooth literal position jump
                marker.setPosition(newPos);

                const gear = truck.GEAR_NUM ?? 'N/A';
                const rpm = truck.ENGINE_SPEED ?? '0';
                const fuel = truck.FUEL_LEVEL ?? '0';
                const def = truck.DEF_LEVEL ?? '0';
                const alt = truck.ALTITUDE ?? '0';
                const speed = truck.SPEED ?? 0;
                const regn = truck.REGN_NUMBER ?? 'Unknown';
                const odo = truck.VEHICLE_ODO ?? 0;

                // Update Icon Colors dynamically
                let lorrySvg = '';
                let popupSvg = '';
                let popupBorder = '';
                if (speed > 0) {
                    lorrySvg = '<svg viewBox="0 0 64 32" xmlns="http://www.w3.org/2000/svg"><rect x="4" y="4" width="38" height="20" rx="3" fill="#43A047"/><path d="M 44 10 L 52 10 C 54 10 56 12 57 14 L 60 19 C 60.5 20 60.5 21 60 22 L 59 24 H 44 Z" fill="#2E7D32"/><path d="M 46 12 L 51 12 C 52 12 53 14 54 14 L 56 18 H 46 Z" fill="#E8F5E9"/><rect x="4" y="23" width="55" height="2" fill="#555"/><circle cx="12" cy="24" r="5" fill="#212121"/><circle cx="24" cy="24" r="5" fill="#212121"/><circle cx="50" cy="24" r="5" fill="#212121"/><circle cx="12" cy="24" r="2" fill="#E0E0E0"/><circle cx="24" cy="24" r="2" fill="#E0E0E0"/><circle cx="50" cy="24" r="2" fill="#E0E0E0"/></svg>';
                    popupSvg = '<svg style="margin-right: 12px; width: 44px; height: 22px;" viewBox="0 0 64 32" xmlns="http://www.w3.org/2000/svg"><rect x="4" y="4" width="38" height="20" rx="3" fill="#43A047"/><path d="M 44 10 L 52 10 C 54 10 56 12 57 14 L 60 19 C 60.5 20 60.5 21 60 22 L 59 24 H 44 Z" fill="#2E7D32"/><path d="M 46 12 L 51 12 C 52 12 53 14 54 14 L 56 18 H 46 Z" fill="#E8F5E9"/><rect x="4" y="23" width="55" height="2" fill="#555"/><circle cx="12" cy="24" r="5" fill="#212121"/><circle cx="24" cy="24" r="5" fill="#212121"/><circle cx="50" cy="24" r="5" fill="#212121"/><circle cx="12" cy="24" r="2" fill="#E0E0E0"/><circle cx="24" cy="24" r="2" fill="#E0E0E0"/><circle cx="50" cy="24" r="2" fill="#E0E0E0"/></svg>';
                    popupBorder = '2px solid #43A047';
                } else {
                    lorrySvg = '<svg viewBox="0 0 64 32" xmlns="http://www.w3.org/2000/svg"><rect x="4" y="4" width="38" height="20" rx="3" fill="#E53935"/><path d="M 44 10 L 52 10 C 54 10 56 12 57 14 L 60 19 C 60.5 20 60.5 21 60 22 L 59 24 H 44 Z" fill="#C62828"/><path d="M 46 12 L 51 12 C 52 12 53 14 54 14 L 56 18 H 46 Z" fill="#E3F2FD"/><rect x="4" y="23" width="55" height="2" fill="#555"/><circle cx="12" cy="24" r="5" fill="#212121"/><circle cx="24" cy="24" r="5" fill="#212121"/><circle cx="50" cy="24" r="5" fill="#212121"/><circle cx="12" cy="24" r="2" fill="#E0E0E0"/><circle cx="24" cy="24" r="2" fill="#E0E0E0"/><circle cx="50" cy="24" r="2" fill="#E0E0E0"/></svg>';
                    popupSvg = '<svg style="margin-right: 12px; width: 44px; height: 22px;" viewBox="0 0 64 32" xmlns="http://www.w3.org/2000/svg"><rect x="4" y="4" width="38" height="20" rx="3" fill="#E53935"/><path d="M 44 10 L 52 10 C 54 10 56 12 57 14 L 60 19 C 60.5 20 60.5 21 60 22 L 59 24 H 44 Z" fill="#C62828"/><path d="M 46 12 L 51 12 C 52 12 53 14 54 14 L 56 18 H 46 Z" fill="#E3F2FD"/><rect x="4" y="23" width="55" height="2" fill="#555"/><circle cx="12" cy="24" r="5" fill="#212121"/><circle cx="24" cy="24" r="5" fill="#212121"/><circle cx="50" cy="24" r="5" fill="#212121"/><circle cx="12" cy="24" r="2" fill="#E0E0E0"/><circle cx="24" cy="24" r="2" fill="#E0E0E0"/><circle cx="50" cy="24" r="2" fill="#E0E0E0"/></svg>';
                    popupBorder = '2px solid #E53935';
                }

                marker.setIcon({
                    url: 'data:image/svg+xml;charset=UTF-8,' + encodeURIComponent(lorrySvg),
                    scaledSize: new google.maps.Size(64, 32),
                    anchor: new google.maps.Point(32, 16),
                    labelOrigin: new google.maps.Point(32, 48)
                });
                marker.setLabel({
                    text: regn,
                    className: speed > 0 ? "truck-marker-label truck-marker-moving" : "truck-marker-label truck-marker-stopped",
                    color: speed > 0 ? "#2E7D32" : "#D32F2F"
                });

                const dateStr = truck.DATE_TIME ? new Date(truck.DATE_TIME).toLocaleString() : 'N/A';
                marker.customHtml = this.buildCustomHtml(speed, gear, rpm, fuel, def, alt, odo, dateStr, regn, popupSvg, popupBorder, 'Fetching location...');

                const cleanRegn = regn.replace(/\W/g, '');
                this.getAddress(lat, lng).then(loc => {
                    marker.customHtml = this.buildCustomHtml(speed, gear, rpm, fuel, def, alt, odo, dateStr, regn, popupSvg, popupBorder, loc);
                    const locElements = document.getElementsByClassName(`loc-text-${cleanRegn}`);
                    for (let i = 0; i < locElements.length; i++) {
                        locElements[i].innerText = loc;
                    }
                });

                // Sync the active popup if it is open
                if (data.activePopup && data.activePopup.position && 
                    Math.abs(data.activePopup.position.lat() - marker.getPosition().lat()) < 0.0001 &&
                    Math.abs(data.activePopup.position.lng() - marker.getPosition().lng()) < 0.0001) {

                    // It's the same truck marker conceptually
                    // Update overlay position and HTML DOM natively
                    data.activePopup.position = newPos;
                    const cDiv = data.activePopup.container.children[1];
                    if (cDiv) {
                        cDiv.innerHTML = marker.customHtml;
                        // Re-attach close button listener since we replaced the HTML
                        let closeBtn = data.activePopup.container.querySelector('.close-popup-btn');
                        if(closeBtn) {
                            closeBtn.addEventListener('click', (e) => {
                                data.activePopup.close();
                                e.stopPropagation();
                            });
                            closeBtn.addEventListener('mouseover', () => closeBtn.style.background = '#e0e0e0');
                            closeBtn.addEventListener('mouseout', () => closeBtn.style.background = '#f0f0f0');
                        }
                    }
                    // Automatically re-draw its position over the active map
                    data.activePopup.draw();
                }
            }
        });
    }
};
