
    $(document).ready(function () {
        $("#Password").prop("disabled", true);
    $("#ConfirmPassword").prop("disabled", true);
    $("#resetButton").prop("disabled", true);

    $("#verifyButton").click(function () {
            var firstName = $("#FirstName").val();
    var lastName = $("#LastName").val();

    $.post("/LoginReg/VerifyUser", {firstName: firstName, lastName: lastName }, function (data) {
                if (data.isValid) {
        $("#Password").prop("disabled", false);
    $("#ConfirmPassword").prop("disabled", false);
    $("#resetButton").prop("disabled", false);
                } else {
        alert("User not found. Please check your first name and last name.");
                }
            });
        });

    $("#resetButton").click(function () {
            var firstName = $("#FirstName").val();
    var lastName = $("#LastName").val();
    var newPassword = $("#Password").val();
    var confirmPassword = $("#ConfirmPassword").val();

    if (newPassword !== confirmPassword) {
        alert("Password and Confirm Password do not match.");
    return;
            }

    $.post("/LoginReg/ResetPassword", {firstName: firstName, lastName: lastName, newPassword: newPassword }, function (data) {
                if (data.success) {

        alert(data.message);

    $("#Password").val("");
    $("#ConfirmPassword").val("");
                } else {

        alert(data.message);
                }
            });
        });
    });
