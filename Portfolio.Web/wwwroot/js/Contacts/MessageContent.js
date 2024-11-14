
$("#myButton").click(function () {
    $(".error").empty(null);
    validation_check();
    if (validation_check()) {
        SendMessage();
    }
});


function validation_check() {
    var isValid = true;
    var Name = $("#name").val();
    var Email = $("#email").val();
    var Message = $("#message").val();
    var Subject = $("#subject").val();
    var Phone = $("#phone").val();

    var NameError = $("#nameError");
    var EmailError = $("#emailError");
    var MessageError = $("#messageError");
    var SubjectError = $("#subjectError");
    var PhoneError = $("#phoneError");

    if (Name === "") {
        NameError.text("Please enter your name");
        $("#name").focus();
        isValid = false;
    }
    else if (Email === "") {
        EmailError.text("Please enter your a valid email");
        $("#email").focus();
        isValid = false;
    }
    else if (Phone === "") {
        PhoneError.text("Please enter your phone No.");
        $("#phone").focus();
        isValid = false;
    }
    else if (Subject === "") {
        SubjectError.text("Please enter subject");
        $("#subject").focus();
        isValid = false;
    }
    else if (Message === "") {
        MessageError.text("Message is required");
        $("#message").focus();
        isValid = false;
    }
    else {
        $("#myButton").css("background-color", "blue");
        $("#myButton").css("color", "white");
        isValid = true;
    }
    return isValid;
}

async function SendMessage() {
    var objContact = {};

    var Name = $("#name").val();
    var Email = $("#email").val();
    var Message = $("#message").val();
    var Subject = $("#subject").val();
    var Phone = $("#phone").val();

    objContact.NAME = Name;
    objContact.SUBJECT = Subject;
    objContact.MESSAGE = Message;
    objContact.EMAIL = Email;
    objContact.PHONE = Phone;

    try {
        const response = await $.ajax({
            url: '/MY_PROFILE/Contact',
            type: "POST",
            dataType: 'json',
            data: { objContact },
        });

        if (response.status === true) {
            alert(response.message);
            $("#myButton").css("background-color", "green");
            $("#myButton").css("color", "white");
        } else {
            alert(response.message);
        }
    } catch (error) {
        alert("Error occurred: " + error.status);
    }
}


