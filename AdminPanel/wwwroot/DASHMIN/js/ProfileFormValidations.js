
$(document).ready(function() {
    // Disable the submit button by default
    $("#AdminProfileSaveChanges").prop("disabled", true);

    // Validate the email field
    $("#AdminEmail").on("input", function() {
        var email = $(this).val();
        if (email.length === 0) {
            $("#AdminEmail").addClass("is-invalid");
        } else {
            $("#AdminEmail").removeClass("is-invalid");
        }
        validateForm();
    });

    // Validate the password field
    $("#AdminOldPassword").on("input", function() {
        var password = $(this).val();
        if (password.length === 0) {
            $("#AdminOldPassword").addClass("is-invalid");
        } else {
            $("#AdminOldPassword").removeClass("is-invalid");
        }
        validateForm();
    });

    // Validate the first name field
    $("#AdminFirstName").on("input", function() {
        var firstName = $(this).val();
        if (firstName.length === 0) {
            $("#AdminFirstName").addClass("is-invalid");
        } else {
            $("#AdminFirstName").removeClass("is-invalid");
        }
        validateForm();
    });

    // Validate the last name field
    $("#AdminELastName").on("input", function() {
        var lastName = $(this).val();
        if (lastName.length === 0) {
            $("#AdminELastName").addClass("is-invalid");
        } else {
            $("#AdminELastName").removeClass("is-invalid");
        }
        validateForm();
    });

    // Validate the date of birth field
    $("#AdminDOB").on("input", function() {
        var dob = $(this).val();
        if (dob.length === 0) {
            $("#AdminDOB").addClass("is-invalid");
        } else {
            $("#AdminDOB").removeClass("is-invalid");
        }
        validateForm();
    });

    // Validate the checkbox
    $("#SaveChangesCheck").on("change", function() {
        validateForm();
    });

    // Function to validate the form and enable/disable the submit button
    function validateForm() {
        var email = $("#AdminEmail").val();
        var password = $("#AdminOldPassword").val();
        var firstName = $("#AdminFirstName").val();
        var lastName = $("#AdminELastName").val();
        var dob = $("#AdminDOB").val();
        var checkboxChecked = $("#SaveChangesCheck").is(":checked");

        if (email.length > 0 && password.length > 0 && firstName.length > 0 && lastName.length > 0 && dob.length > 0 && checkboxChecked) {
            $("#AdminProfileSaveChanges").prop("disabled", false);
        } else {
            $("#AdminProfileSaveChanges").prop("disabled", true);
        }
    }
});
