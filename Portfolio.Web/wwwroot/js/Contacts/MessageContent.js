
const $Name = $("#name");
const $Email = $("#email");
const $Message = $("#message");
const $Subject = $("#subject");
const $Phone = $("#phone");
const $NameError = $("#nameError");
const $EmailError = $("#emailError");
const $MessageError = $("#messageError");
const $SubjectError = $("#subjectError");
const $PhoneError = $("#phoneError");
const $Button = $("#myButton");
const $Error = $(".error");

$Button.on("click", function () {
    $Error.empty(null);
    if (IsValid()) {
        SendMessage();
    }
});


function IsValid() {
    var isValid = true;

    if ($Name.val() === "") {
        $NameError.text("Name is required");
        $Name.trigger("focus");
        isValid = false;
    }
    else if ($Email.val() === "") {
        $EmailError.text("Email is required");
        $Email.trigger("focus");
        isValid = false;
    }
    else if ($Phone.val() === "") {
        $PhoneError.text("Phone is required");
        $Phone.trigger("focus");
        isValid = false;
    }
    else if ($Subject.val() === "") {
        $SubjectError.text("Please enter subject of your email");
        $Subject.trigger("focus");
        isValid = false;
    }
    else if ($Message.val() === "") {
        $MessageError.text("Message is required");
        $Message.trigger("focus");
        isValid = false;
    }
    else {
        $Button.css("background-color", "blue");
        $Button.css("color", "white");
        isValid = true;
    }
    return isValid;
}

function getLocation() {
    return new Promise((resolve, reject) => {
        if (!navigator.geolocation) {
            reject("NOT_SUPPORTED");
            return;
        }
        navigator.geolocation.getCurrentPosition(
            (position) => resolve(position),
            (error) => reject(error),
            { enableHighAccuracy: true, timeout: 10000 }
        );
    });
}

async function SendMessage() {
    let latitude = "";
    let longitude = "";

    try {
        const position = await getLocation();
        latitude = position.coords.latitude.toString();
        longitude = position.coords.longitude.toString();
    } catch (error) {
        let msg = "Location access is required to send a message.";

        if (error === "NOT_SUPPORTED") {
            msg = "Your browser does not support geolocation. Please use a modern browser.";
        } else if (error.code === error.PERMISSION_DENIED) {
            msg = "You have denied location access. We need your location to process your message.";
        } else if (error.code === error.POSITION_UNAVAILABLE) {
            msg = "Your location information is unavailable at the moment. Please try again.";
        } else if (error.code === error.TIMEOUT) {
            msg = "Location request timed out. Please check your connection and try again.";
        }

        Swal.fire({
            icon: 'warning',
            title: 'Location Required',
            text: msg,
            confirmButtonText: 'OK'
        }).then(() => {
            window.location.href = '/';
        });
        return;
    }

    let objContact = {};

    objContact.NAME = $Name.val();
    objContact.SUBJECT = $Subject.val();
    objContact.MESSAGE = $Message.val();
    objContact.EMAIL = $Email.val();
    objContact.PHONE = $Phone.val();
    objContact.LATITUDE = latitude;
    objContact.LONGITUTE = longitude;

    try {
        const response = await $.ajax({
            url: '/MY_PROFILE/Contact',
            type: "POST",
            dataType: 'json',
            data: { objContact },
        });

        if (response.status === true) {
            alert(response.message);
            $Button.css("background-color", "green");
            $Button.css("color", "white");
        } else {
            alert(response.message);
        }
    } catch (error) {
        console.log("Error occurred: ", error);
    }
}


