(function ($) {
    "use strict";
    $(document).on('ready', function () {
        var isLoggedIn = $('body').attr('data-loggedin') === "1";

        if (isLoggedIn) {
            Auth.RegisterLogout();
            Auth.RegisterRefreshToken();
            Cart.GetCurrentQuantity();
        }

        Cart.RegisterAddToCart();
    });
})(jQuery);
