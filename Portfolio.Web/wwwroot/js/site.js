// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//on load heading color change effect
window.addEventListener('load', function () {
    typeHeadingWriter();
    toggleFilterButton();
    getLocation();
    pageFx();
    //setInterval(changeHeadingColor, 5000); // 5000 milliseconds = 5 seconds
});

/* Click ripples + cursor trail.
   - On click: two expanding neon rings + a radial particle burst at
     the click location, colour cycled through a curated palette.
   - On mousemove (throttled): a single small particle falling downward
     so the cursor leaves a faint sparkling trail.
   - Skipped on touch devices and when the user prefers reduced motion. */
const pageFx = function () {
    if (window.matchMedia('(hover: none)').matches) return;
    if (window.matchMedia('(prefers-reduced-motion: reduce)').matches) return;

    const COLORS = ['#bef264', '#22d3ee', '#f472b6', '#a78bfa', '#fb923c'];
    let colorIdx = 0;
    const nextColor = () => COLORS[(colorIdx++) % COLORS.length];

    function spawn(cls, x, y, color, vars) {
        const el = document.createElement('div');
        el.className = cls;
        el.style.left = x + 'px';
        el.style.top = y + 'px';
        el.style.setProperty('--fx-c', color);
        if (vars) {
            for (const k in vars) el.style.setProperty(k, vars[k]);
        }
        document.body.appendChild(el);
        el.addEventListener('animationend', () => el.remove(), { once: true });
        // Safety net in case animationend doesn't fire
        setTimeout(() => el.remove(), 1200);
    }

    /* Click — water-drop ripple + radial burst */
    document.addEventListener('click', (e) => {
        // Ignore clicks on interactive SVG/canvas (e.g. chart libraries) — the
        // ripple still looks fine but we skip on form controls that might
        // conflict with their own focus animations.
        const c = nextColor();
        spawn('fx-ripple', e.clientX, e.clientY, c);

        const count = 10;
        for (let i = 0; i < count; i++) {
            const angle = (Math.PI * 2 * i) / count + (Math.random() - 0.5) * 0.4;
            const dist  = 50 + Math.random() * 40;
            spawn('fx-burst', e.clientX, e.clientY, nextColor(), {
                '--dx': Math.cos(angle) * dist + 'px',
                '--dy': Math.sin(angle) * dist + 'px',
            });
        }
    });

    /* Mousemove — throttled downward-drift trail particles */
    let lastSpawn = 0;
    document.addEventListener('mousemove', (e) => {
        const now = performance.now();
        if (now - lastSpawn < 45) return;
        lastSpawn = now;

        const dx = (Math.random() - 0.5) * 24;
        const dy = 10 + Math.random() * 20;
        spawn('fx-trail', e.clientX, e.clientY, nextColor(), {
            '--dx': dx + 'px',
            '--dy': dy + 'px',
        });
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