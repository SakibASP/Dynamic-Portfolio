const welcomeMessage = document.getElementById("coverName").value;
const mainMessage = document.getElementById("coverDesc").value;
const welcomeElement = document.getElementById('welcome');
const messageElement = document.getElementById('message');
const welcomeElementMobile = document.getElementById('welcomeMobile');
const messageElementMobile = document.getElementById('messageMobile');

//-------------DESKTOP VIEW--------------------
function typeWriter(index, text, element) {
    if (index < text.length) {
        element.textContent += text[index];
        index++;
        setTimeout(() => {
            typeWriter(index, text, element);
        }, 100);
    } else {
        setTimeout(() => {
            element.textContent = ''; // Clear the text content
            //welcomeElement.textContent = '';
            //messageElement.textContent = '';
            typeWriter(0, text, element); // Restart typing
        }, 15000); // Delay before starting again (15 seconds)
    }
}

// Start typing the welcome message
typeWriter(0, welcomeMessage, welcomeElement);

// After the welcome message is typed, start typing the main message
setTimeout(() => {
    //welcomeElement.textContent = ''; // Clear the text content of welcomeElement
    typeWriter(0, mainMessage, messageElement);
}, welcomeMessage.length * 100 + 1000); // Additional delay after welcome message


//------------------MBILE VIEW------------------
function typeWriterMobile(index, text, element) {
    if (index < text.length) {
        element.textContent += text[index];
        index++;
        setTimeout(() => {
            typeWriterMobile(index, text, element);
        }, 100);
    } else {
        setTimeout(() => {
            element.textContent = ''; // Clear the text content
            //messageElementMobile.textContent = '';
            typeWriterMobile(0, text, element); // Restart typing
        }, 15000); // Delay before starting again (15 seconds)
    }
}

// Start typing the welcome message
typeWriterMobile(0, welcomeMessage, welcomeElementMobile);

// After the welcome message is typed, start typing the main message
setTimeout(() => {
    //welcomeElement.textContent = ''; // Clear the text content of welcomeElement
    typeWriterMobile(0, mainMessage, messageElementMobile);
}, welcomeMessage.length * 100 + 1000); // Additional delay after welcome message