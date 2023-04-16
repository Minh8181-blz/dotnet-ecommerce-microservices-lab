var Checkout = {};

Checkout.Init = function (callback) {
    $.ajax({
        url: '/checkout',
        method: 'post',
        success: function (data) {
            if (callback instanceof Function) {
                callback(data);
            }
        }
    });
}