
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
        $NameError.text("Please enter your name");
        $Name.trigger("focus");
        isValid = false;
    }
    else if ($Email.val() === "") {
        $EmailError.text("Please enter your a valid email");
        $Email.trigger("focus");
        isValid = false;
    }
    else if ($Phone.val() === "") {
        $PhoneError.text("Please enter your phone No.");
        $Phone.trigger("focus");
        isValid = false;
    }
    else if ($Subject.val() === "") {
        $SubjectError.text("Please enter subject");
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

async function SendMessage() {
    let objContact = {};

    objContact.NAME = $Name.val();
    objContact.SUBJECT = $Subject.val();
    objContact.MESSAGE = $Message.val();
    objContact.EMAIL = $Email.val();
    objContact.PHONE = $Phone.val();

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


