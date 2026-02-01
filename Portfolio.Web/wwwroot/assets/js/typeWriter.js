document.addEventListener('DOMContentLoaded', function () {
    const coverNameInput = document.getElementById("coverName");
    const coverDescInput = document.getElementById("coverDesc");

    if (!coverNameInput || !coverDescInput) return;

    const welcomeMessage = coverNameInput.value;
    const mainMessage = coverDescInput.value;
    const welcomeElement = document.getElementById('welcome');
    const messageElement = document.getElementById('message');
    const welcomeElementMobile = document.getElementById('welcomeMobile');
    const messageElementMobile = document.getElementById('messageMobile');

    function typeWriter(index, text, element) {
        if (!element) return;
        if (index < text.length) {
            element.textContent += text[index];
            index++;
            setTimeout(() => {
                typeWriter(index, text, element);
            }, 100);
        } else {
            setTimeout(() => {
                element.textContent = '';
                typeWriter(0, text, element);
            }, 15000);
        }
    }

    if (welcomeElement) {
        typeWriter(0, welcomeMessage, welcomeElement);
    }

    if (messageElement) {
        setTimeout(() => {
            typeWriter(0, mainMessage, messageElement);
        }, welcomeMessage.length * 100 + 1000);
    }

    if (welcomeElementMobile) {
        typeWriter(0, welcomeMessage, welcomeElementMobile);
    }

    if (messageElementMobile) {
        setTimeout(() => {
            typeWriter(0, mainMessage, messageElementMobile);
        }, welcomeMessage.length * 100 + 1000);
    }
});