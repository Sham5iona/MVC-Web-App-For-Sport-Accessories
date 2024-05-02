$(document).ready(function () {
    $('#reset').prop('disabled', true);

    $('#imageUpload').change(function () {
        var input = this;
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imagePreview img').attr('src', e.target.result);
            $('#reset').prop('disabled', false);
        };

        if (input.files && input.files[0]) {
            reader.readAsDataURL(input.files[0]); // Convert to base64 string
        } else {
            resetImagePreview();
        }
    });

    $('#reset').click(function () {
        resetImagePreview();
        $('#imageUpload').val('');
        $('#reset').prop('disabled', true);
    });

    // Function to reset the image preview
    function resetImagePreview() {
        $('#imagePreview img').attr('src', $('#originalImageUrl').val());
    }
});


function CheckEmailValue() {

    let is_checked = document.getElementById("emailConfirmed").checked;

    if (is_checked) {

        document.getElementById("emailConfirmed").value = true;

    } else {

        document.getElementById("emailConfirmed").value = false;

    }
}

function CheckLockoutValue() {

    let is_checked = document.getElementById("lockoutEnabled").checked;

    if (is_checked) {

        document.getElementById("lockoutEnabled").value = true;

    } else {

        document.getElementById("lockoutEnabled").value = false;

    }
}

function Check2FAValue() {

    let is_checked = document.getElementById("twoFactorEnabled").checked;

    if (is_checked) {

        document.getElementById("twoFactorEnabled").value = true;

    } else {

        document.getElementById("twoFactorEnabled").value = false;

    }
}
