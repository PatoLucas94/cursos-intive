(function() {

    // Logical OR
    const lat = document.querySelector('#lat').value || -34.5635042; // truti o falsy
    const lng = document.querySelector('#lng').value || -58.4663525;
    const mapa = L.map('mapa').setView([lat, lng ], 13);
    let marker;
    
    // Utilizar provider y GeoCoder
    const geocodeService = L.esri.Geocoding.geocodeService();

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(mapa);

    // el pin
    marker = new L.marker([lat, lng], {
        draggable: true,
        autoPan: true
    })
    .addTo(mapa)

    // Detectar el movimiento del pin
    marker.on('moveend', function(e){
        marker = e.target

        const posicion = marker.getLatLng();

        mapa.panTo(new L.latLng(posicion.lat, posicion.lng)) // Centramos el marker donde yo suelte

        // Obtener la info de las calles al soltar el pin

        geocodeService.reverse().latlng(posicion, 13).run(function(error, resultado){
            marker.bindPopup(resultado.address.LongLabel)

        // Llenar los parrafos con la info de la calle, lat, lng

        document.querySelector('.calle').textContent = resultado?.address?.Address ?? '';
        // Lo que agregamos en la base de datos que el usuario no ve
        document.querySelector('#calle').value = resultado?.address?.Address ?? ''; 

        document.querySelector('#lat').textContent = resultado?.latlng?.lat ?? '';
        document.querySelector('#lng').value = resultado?.latlng?.lng ?? '';

        })

        
    })


})()