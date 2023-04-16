(function ($) {
    function OrderDetailsPage() {
        $('#pay').click(function () {
            var orderId = $(this).attr('data-order-id');
            Payment.PayWithStripe(orderId, function (data) {
                location.href = data.paymentInstruction.url;
            });
        });
    }

    $(document).ready(function () {
        new OrderDetailsPage();
    });
})(jQuery);

