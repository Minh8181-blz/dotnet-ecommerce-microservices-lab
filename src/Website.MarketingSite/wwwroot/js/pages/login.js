(function ($) {
    function LogIn() {
        var form = $('#frm_login');
        var errorMsgElm = $('#frm_login_error_msg');
        var successMsgElm = $('#frm_login_success_msg');

        form.submit(function (e) {
            e.preventDefault();

            successMsgElm.addClass('d-none');
            errorMsgElm.addClass('d-none');

            if (validateForm()) {
                console.log('ok');

                FormSubmitter.SubmitUrlEncodedForm(form, function (success, data) {
                    if (success) {
                        console.log('s', data);
                        successMsgElm.removeClass('d-none').html('Login success');

                        setTimeout(function () {
                            window.location.href = "/";
                        }, 300);
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
        new LogIn();
    });
})(jQuery);

