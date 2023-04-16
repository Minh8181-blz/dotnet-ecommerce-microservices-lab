(function ($) {
    function CartPage() {
        var cartElm = $('#myCart');
        var totalAmountElm = $('#total');

        cartElm.on('click', '.cart-qty-subtract', function () {
            var self = $(this);
            var prdId = parseInt(self.attr('data-product-id'));
            var currentQty = parseInt($(`#item${prdId}`).find('.cart-qty').text());

            if (currentQty === 1)
                return;

            Cart.UpdateCart(CART_ACTIONS.DecreaseItemQuantity, prdId, function (result) {
                if (result.succeeded) {
                    var item = result.cart.cartItems.find(x => x.productId === prdId);
                    UpdateCartItemUI(item);
                    UpdateTotal(result.cart.totalPrice);
                }
            });
        });

        cartElm.on('click', '.cart-qty-add', function () {
            var self = $(this);
            var prdId = parseInt(self.attr('data-product-id'));

            Cart.UpdateCart(CART_ACTIONS.AddItemQuantity, prdId, function (result) {
                if (result.succeeded) {
                    var item = result.cart.cartItems.find(x => x.productId === prdId);
                    UpdateCartItemUI(item);
                    UpdateTotal(result.cart.totalPrice);
                }
            });
        });

        cartElm.on('click', '.cart-remove-item', function () {
            var self = $(this);
            var prdId = parseInt(self.attr('data-product-id'));

            Cart.UpdateCart(CART_ACTIONS.RemoveItem, prdId, function (result) {
                if (result.succeeded) {
                    var cart = result.cart;
                    if (!cart) {
                        Cart.DisplayQuantity('');
                        DisplayEmptyCart();
                    }
                    else if (cart.status === CART_STATUS.Abandoned || cart.length === 0) {
                        Cart.DisplayQuantity('');
                        DisplayEmptyCart();
                    }
                    else {
                        var item = cart.cartItems.find(x => x.productId === prdId);
                        if (!item) {
                            $(`#item${prdId}`).remove();
                            UpdateTotal(cart.totalPrice);
                            Cart.DisplayQuantity(cart.cartItems.length);
                        }
                    }
                }
            });
        });

        var btnCheckout = $('#btnCheckout');
        if (btnCheckout.length > 0) {
            btnCheckout.click(function () {
                btnCheckout.prop('disabled', true);
                Checkout.Init(function (data) {
                    if (data.succeeded) {
                        $('#notifyModal').modal('show');
                        btnCheckout.prop('disabled', false);
                    }
                });
            });
        }

        $('#notifyModal').on('hidden.bs.modal', function () {
            location.href = "/orders";
        });

        function UpdateCartItemUI(item) {
            $(`#item${item.productId} .cart-qty`).text(item.quantity);
            $(`#item${item.productId} .cart-item-total`).text(`$${priceUtils.formatPrice(item.unitPrice * item.quantity)}`);
        }

        function UpdateTotal(price) {
            totalAmountElm.html(`$${priceUtils.formatPrice(price)}`);
        }

        function DisplayEmptyCart() {
            cartElm.addClass('d-none');
            $('#emptyCart').removeClass('d-none');
        }
    }

    $(document).ready(function () {
        new CartPage();
    });
})(jQuery);

