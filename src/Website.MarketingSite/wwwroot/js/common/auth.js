var Auth = {};

Auth.RegisterLogout = function () {
    var form = $('#frmLogout');

    if (form.length > 0) {
        form.find('a').click(function (e) {
            e.preventDefault();
            FormSubmitter.SubmitUrlEncodedForm(form, function (success) {
                if (success) {
                    window.location.href = "/";
                }
            });
        });
    }
};

Auth.RegisterRefreshToken = function () {
    checkAndGetCookie();
    setInterval(checkAndGetCookie, 300000);

    function checkAndGetCookie() {
        console.log('check access token exp at', new Date().toISOString());
        var accessTokenExpTimeCookie = CookieUtils.GetCookie("access_token_expire_time");

        if (accessTokenExpTimeCookie !== '') {
            var tokenExpTime = new Date(accessTokenExpTimeCookie);

            if (tokenExpTime - new Date() < 310000) {
                console.log('fetching cookie...');
                $.get('/refresh-token');
            }
        }
        else {
            console.log('deleting cookie...');
            CookieUtils.DeleteCookie("access_token_expire_time");
        }
    }
};