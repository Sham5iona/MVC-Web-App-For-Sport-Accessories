
$("#submitButton").prop('disabled', true);
    $('#imageUpload').change(function () {
        var input = this;
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imagePreview img').attr('src', e.target.result);
            $("#submitButton").prop('disabled', false);
            
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
    });

    // Function to reset the image preview
    function resetImagePreview() {
        $('#imagePreview img').attr('src', $('#originalImageUrl').val());
        $("#submitButton").prop('disabled', true);
    }


