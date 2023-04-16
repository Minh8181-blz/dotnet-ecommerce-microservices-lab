var Cart = {};

Cart.RegisterAddToCart = function () {
    $('.add-to-cart').click(function () {
        var self = $(this);
        var prdId = parseInt(self.attr('data-product-id'));

        Cart.AddToCart(prdId, 1, function (result) {
            if (result && result.succeeded) {
                Cart.DisplayQuantity(result.cart.cartItems.length);
            }
        });
    });
}

Cart.GetCurrentQuantity = function () {
    Cart.GetCartDetails(function (cart) {
        quantity = cart?.cartItems?.length;

        if (!isNaN(quantity) && +quantity > 0) {
            Cart.DisplayQuantity(quantity);
        }
    });
};

Cart.GetCartDetails = function (callback) {
    $.ajax({
        url: '/cart/api/my-cart',
        success: function (cart) {
            if (callback instanceof Function) {
                callback(cart);
            }
        }
    });
};

Cart.DisplayQuantity = function (quantity) {
    var qtyElm = $('#menuCartQty');
    if (quantity === '' || quantity === 0) {
        qtyElm.html('').addClass('d-none');
    }
    else {
        qtyElm.html(quantity).removeClass('d-none');
    }
};

Cart.AddToCart = function (productId, quantity, callback) {
    $.ajax({
        url: '/cart/api/add-to-cart',
        method: 'post',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            productId: productId,
            quantity: quantity
        }),
        success: function (result) {
            if (callback instanceof Function) {
                callback(result);
            }
        }
    });
};

Cart.UpdateCart = function (action, productId, callback) {
    $.ajax({
        url: '/cart/api/update-cart',
        method: 'post',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            action: action,
            productId: productId
        }),
        success: function (result) {
            if (callback instanceof Function) {
                callback(result);
            }
        }
    });
};