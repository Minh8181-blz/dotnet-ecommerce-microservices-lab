(function ($) {
    function SignUp() {
        var form = $('#frm_signup');
        var errorMsgElm = $('#frm_signup_error_msg');
        var successMsgElm = $('#frm_signup_success_msg');

        form.submit(function (e) {
            e.preventDefault();

            successMsgElm.addClass('d-none');
            errorMsgElm.addClass('d-none');

            if (validateForm()) {
                console.log('ok');

                FormSubmitter.SubmitUrlEncodedForm(form, function (success, data) {
                    if (success) {
                        console.log('s', data);
                        successMsgElm.removeClass('d-none').html('Your account has been created');
                        window.location.href = "/";
                    }
                    else {
                        console.log('f', data);
                        errorMsgElm.removeClass('d-none').html(data.responseText);
                    }
                });
            }
            else {
                console.log('not ok');
            }
        });

        function validateForm() {
            return form.valid();
        }
    }

    $(document).ready(function () {
        new SignUp();
    });
})(jQuery);

