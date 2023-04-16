var Payment = {};

Payment.PayWithStripe = function (orderId, callback) {
    $.ajax({
        url: '/payments/stripe/pay/' + orderId,
        type: 'POST',
        success: function (data) {
            if (callback instanceof Function) {
                callback(data);
            }
        }
    });
};