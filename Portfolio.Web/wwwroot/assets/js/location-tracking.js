const HOME_PATHS = ['/', '/home', '/home/index'];

function isHomePage() {
    return HOME_PATHS.includes(window.location.pathname.toLowerCase());
}

function isLocationDenied() {
    return sessionStorage.getItem('locationDenied') === 'true';
}

function blockNavLinks() {
    document.querySelectorAll('#navbar .nav-item a').forEach(function (link) {
        const href = (link.getAttribute('href') || '').toLowerCase();
        if (href && !HOME_PATHS.includes(href)) {
            link.addEventListener('click', async function (e) {
                e.preventDefault();
                try {
                    const status = await navigator.permissions.query({ name: 'geolocation' });
                    if (status.state === 'granted') {
                        sessionStorage.setItem('locationDenied', 'false');
                        window.location.href = href;
                    } else {
                        sessionStorage.setItem('locationDenied', 'true');
                        Swal.fire({
                            icon: 'info',
                            title: 'Location Permission Required',
                            html: 'You need to allow location access to browse this page.<br><br>' +
                                'Please enable location permissions in your browser settings and <b>refresh</b> the page.',
                            confirmButtonText: 'OK',
                            confirmButtonColor: '#3085d6'
                        }).then(() => {
                            location.reload();
                        });
                    }
                } catch {
                    if (isLocationDenied()) {
                        Swal.fire({
                            icon: 'info',
                            title: 'Location Permission Required',
                            html: 'You need to allow location access to browse this page.<br><br>' +
                                'Please enable location permissions in your browser settings and <b>refresh</b> the page.',
                            confirmButtonText: 'OK',
                            confirmButtonColor: '#3085d6'
                        }).then(() => {
                            location.reload();
                        });
                    } else {
                        window.location.href = href;
                    }
                }
            });
        }
    });
}

async function getLocation() {
    // Block nav links on every call so denied users can't navigate
    blockNavLinks();

    // If location was already denied and user somehow landed on a non-home page, redirect
    if (isLocationDenied() && !isHomePage()) {
        window.location.href = '/';
        return;
    }

    if (!window.isSecureContext) {
        sessionStorage.setItem('locationDenied', 'true');
        Swal.fire({
            icon: 'error',
            title: 'Secure Connection Required',
            html: 'This portfolio requires a secure connection (HTTPS) to function properly.<br>' +
                'Please visit using <b>HTTPS</b> to access all features.',
            confirmButtonColor: '#d33'
        });
        if (!isHomePage()) window.location.href = '/';
        return;
    }

    if (!navigator.geolocation) {
        sessionStorage.setItem('locationDenied', 'true');
        Swal.fire({
            icon: 'error',
            title: 'Browser Not Supported',
            html: 'Your browser does not support geolocation.<br>' +
                'Please use a modern browser (Chrome, Firefox, Edge) to access all pages of this portfolio.',
            confirmButtonColor: '#d33'
        });
        if (!isHomePage()) window.location.href = '/';
        return;
    }

    navigator.geolocation.getCurrentPosition(
        async (position) => {
            const latitude = position.coords.latitude;
            const longitude = position.coords.longitude;

            // Location granted — clear the denied flag
            sessionStorage.setItem('locationDenied', 'false');

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
                    // Location saved successfully
                }
            } catch (err) {
                console.error('Error saving location:', err);
            }
        },
        (error) => {
            sessionStorage.setItem('locationDenied', 'true');

            if (error.code === error.PERMISSION_DENIED) {
                Swal.fire({
                    icon: 'warning',
                    title: 'Location Access Required',
                    html: 'This portfolio uses your location to track visitors.<br><br>' +
                        'Without location permission, you can only view the <b>Home</b> page.<br>' +
                        'To access all pages (About, Resume, Projects, Contact, etc.), ' +
                        'please <b>allow location access</b> in your browser settings and refresh the page.',
                    confirmButtonText: 'Stay on Home',
                    confirmButtonColor: '#f0ad4e'
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Location Unavailable',
                    html: 'We couldn\'t determine your location due to a technical issue.<br><br>' +
                        'Without location access, you can only view the <b>Home</b> page.<br>' +
                        'Please check your device\'s location settings and try again.',
                    confirmButtonText: 'Stay on Home',
                    confirmButtonColor: '#d33'
                });
            }

            // Redirect non-home pages back to home
            if (!isHomePage()) {
                window.location.href = '/';
            }
        }
    );
}
