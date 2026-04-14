// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//on load heading color change effect
window.addEventListener('load', function () {
    typeHeadingWriter();
    toggleFilterButton();
    getLocation();
    cursorSpotlight();
    //setInterval(changeHeadingColor, 5000); // 5000 milliseconds = 5 seconds
});

/* Site-wide cursor spotlight — updates the --mx / --my CSS custom
   properties on <html> so dark.css can render the radial gradient that
   tracks the mouse. Skips on touch devices; rAF-throttled. */
const cursorSpotlight = function () {
    if (window.matchMedia("(hover: none)").matches) return;
    const root = document.documentElement;
    let x = 0, y = 0, ticking = false;
    function apply() {
        root.style.setProperty('--mx', x + 'px');
        root.style.setProperty('--my', y + 'px');
        ticking = false;
    }
    window.addEventListener('mousemove', (e) => {
        x = e.clientX;
        y = e.clientY;
        if (!ticking) { requestAnimationFrame(apply); ticking = true; }
    }, { passive: true });
};

//Type writing element
const typeHeadingWriter = function () {
    const heading = document.getElementById("changing-heading");
    if (heading) {
        const text = heading.textContent; // Text you want to display

        let index = 0;

        heading.textContent = "";
        function typeWriter() {
            if (index < text.length) {
                heading.innerHTML += text.charAt(index);
                index++;
                setTimeout(typeWriter, 100); // Adjust typing speed (milliseconds)
            }
        }

        typeWriter();
    }
}

const toggleFilterButton = function () {
    const filterToggleButton = document.querySelector('#filterToggleButton');
    const filterFormContainer = document.querySelector('#filterFormContainer');
    const filterToggleIcon = document.querySelector('#filterToggleIcon');

    if (filterToggleButton && filterFormContainer && filterToggleIcon) {
        filterToggleButton.addEventListener('click', function () {
            if (filterFormContainer.classList.contains('show')) {
                filterFormContainer.classList.remove('show');
                filterToggleIcon.classList.remove('bi-minus');
                filterToggleIcon.classList.add('bi-plus');
                setTimeout(() => {
                    filterFormContainer.style.display = 'none';
                }, 400);
            } else {
                filterToggleIcon.classList.remove('bi-plus');
                filterToggleIcon.classList.add('bi-minus');
                filterFormContainer.style.display = 'block';
                setTimeout(() => {
                    filterFormContainer.classList.add('show');
                }, 10);
            }
        });
    }
}