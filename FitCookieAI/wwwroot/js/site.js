$(document).ready(function () {
    $('#authenticated').hide();
    $('#sign-up-authentication-error').hide();
    $('#login-authentication-error').hide();
    $('#authenticated').hide();
    $("#btn-next-step-1").hide();

    /////////////////////////////////// Login and SignUp Validations ///////////////////////////////////
    function validateEmail(email) {
        var emailRegex = /^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$/;
        return emailRegex.test(email);
    }

    function validatePassword(password) {
        return password.length >= 6;
    }

    function validateNotEmpty(value) {
        return value.trim().length > 0;
    }

    function validateInput(input, showError) {
        var isValid = true;
        var errorElement = $('#' + input.attr('id') + '-validation');

        if (!validateNotEmpty(input.val())) {
            isValid = false;
            if (showError) {
                errorElement.text('This field is required.');
            }
        } else if (input.attr('type') === 'email' && !validateEmail(input.val())) {
            isValid = false;
            if (showError) {
                errorElement.text('Please enter a valid email address.');
            }
        } else if (input.attr('type') === 'password' && !validatePassword(input.val())) {
            isValid = false;
            if (showError) {
                errorElement.text('Password must be at least 6 characters.');
            }
        } else {
            errorElement.text('');
        }

        return isValid;
    }

    function validateForm(formSelector, buttonSelector) {
        var isValid = true;
        $(formSelector + ' input[required]').each(function () {
            if (!validateInput($(this), false)) {
                isValid = false;
            }
        });

        if (formSelector === '#noAccount') {
            var password = $('#password');
            var confirmPassword = $('#confirmPassword');
            if (password.val() !== confirmPassword.val()) {
                isValid = false;
                if (password.val().length > 0 && confirmPassword.val().length > 0) {
                    $('#confirmPassword-validation').text('Passwords do not match.');
                }
            } else {
                $('#confirmPassword-validation').text('');
            }
        }

        $(buttonSelector).prop('disabled', !isValid);
    }

    $('#btn-sign-up').hide();

    $('input[type=radio][name=HasAccount]').change(function () {
        if (this.value == 'true') {
            $('#hasAccount').show();
            $('#noAccount').hide();

            $('#btn-login').show();
            $('#btn-sign-up').hide();
        }
        else if (this.value == 'false') {
            $('#hasAccount').hide();
            $('#noAccount').show();

            $('#btn-login').hide();
            $('#btn-sign-up').show();
        }

        validateForm('#hasAccount', '#btn-login');
        validateForm('#noAccount', '#btn-sign-up');
    });

    $('#hasAccount input[required], #noAccount input[required]').on('input', function () {
        validateForm('#hasAccount', '#btn-login');
        validateForm('#noAccount', '#btn-sign-up');
    });

    validateForm('#hasAccount', '#btn-login');
    validateForm('#noAccount', '#btn-sign-up');

    /////////////////////////////////// Login and SignUp Validations ///////////////////////////////////

    ///////////////////////////// Calculate BMI ////////////////////////////////////////////////////////

    function updateBMI() {
        const weight = parseFloat($('#weight').val());
        const height = parseFloat($('#height').val()) / 100; // convert cm to m

        if (!isNaN(weight) && !isNaN(height) && height > 0) {
            const bmi = weight / (height * height);
            $('#BMI').val(bmi.toFixed(2));
        } else {
            $('#BMI').val('');
        }
    }

    $('#weight, #height').on('input', updateBMI);

    ///////////////////////////// Calculate BMI /////////////////////////////////////////////////////////

    ////////////////////////////////////////// Form validations /////////////////////////////////////////
    function dietPlanValidations() {
        var weight = parseFloat($('#weight').val());
        var targetWeight = parseFloat($('#targetWeight').val());
        const height = parseFloat($('#height').val());

        $('#btn-next-step-2').prop('disabled', true);
        $('#btn-next-step-3').prop('disabled', true);
        $('#btn-next-step-4').prop('disabled', true);

        //step 2
        if (isNaN(weight) || isNaN(targetWeight) || weight <= 0 || targetWeight <= 0) {
            $('#btn-next-step-2').prop('disabled', true);
        }
        else {
            $('#btn-next-step-2').prop('disabled', false);
        }

        //step 3
        if (isNaN(height) || height <= 0) {
            $('#btn-next-step-3').prop('disabled', true);
        }
        else {
            $('#btn-next-step-3').prop('disabled', false);
        }

        //step 4
        if ($('#ocupation').val() == '' || $('#ocupation').val() == null) {
            $('#btn-next-step-4').prop('disabled', true);
        }
        else {
            $('#btn-next-step-4').prop('disabled', false);
        }
    }

    dietPlanValidations();

    $('#weight, #targetWeight').on('input', dietPlanValidations);
    $('#height').on('input', dietPlanValidations);
    $('#ocupation').on('input', dietPlanValidations);
    ////////////////////////////////////////// Form validations /////////////////////////////////////////
});