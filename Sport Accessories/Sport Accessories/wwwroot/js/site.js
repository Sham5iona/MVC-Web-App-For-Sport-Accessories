
$("#submitButton").prop('disabled', true);
$('#reset').prop('disabled', true);
    $('#imageUpload').change(function () {
        var input = this;
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imagePreview img').attr('src', e.target.result);
            $("#submitButton").prop('disabled', false);
            $('#reset').prop('disabled', false);
        };

        if (input.files && input.files[0]) {

            reader.readAsDataURL(event.target.files[0]); // Convert to base64 string
            

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
        $("#submitButton").prop('disabled', true);
}
$('#ShowUsernameForm').click(function () {
    $('#FormChangeUsername').toggle();
});

$('#ShowPasswordForm').click(function () {
    $('#ChangePasswordForm').toggle();
});


//show the change username form for always when the span validation displays an error
if ($('#usernameValidationError').text().trim() !== '') {
    $('#FormChangeUsername').show();
}
