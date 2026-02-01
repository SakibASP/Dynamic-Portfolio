async function getLocation() {
    if (!window.isSecureContext) {
        Swal.fire({
            icon: 'error',
            title: 'HTTPS Required',
            html: 'Geolocation requires a secure connection (HTTPS or localhost).<br>' +
                'Current URL: <b>' + window.location.origin + '</b>'
        });
        return;
    }
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
            async (position) => {
                const latitude = position.coords.latitude;
                const longitude = position.coords.longitude;

                const _body = JSON.stringify({
                    latitude: latitude.toString(),
                    longitude: longitude.toString()
                });

                try {
                    const response = await fetch('/Home/SaveLocation', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        body: _body
                    });

                    const result = await response.json();

                    if (result && result.success) {
                        // success handling if needed
                    }
                } catch (err) {
                    console.error('Error saving location:', err);
                }
            },
            (error) => {
                if (error.code === error.PERMISSION_DENIED) {
                    Swal.fire({
                        icon: 'warning',
                        title: 'Location Access Denied',
                        html: 'You denied location access.<br><br>' +
                            'To allow it, click the <b>lock/info icon</b> in the address bar,<br>' +
                            'set <b>Location</b> to <b>Allow</b>, then reload the page.'
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Location unavailable or another issue occurred.'
                    });
                }
            }
        );
    } else {
        Swal.fire({
            icon: 'error',
            title: 'Not Supported',
            text: 'Geolocation is not supported by this browser.'
        });
    }
}
