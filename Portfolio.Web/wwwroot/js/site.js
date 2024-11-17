// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//on load heading color change effect
window.addEventListener('load', function () {
    typeHeadingWriter();
    toggleFilterButton();
    //setInterval(changeHeadingColor, 5000); // 5000 milliseconds = 5 seconds
});

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