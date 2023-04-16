var priceUtils = {
    formatPrice: function (price) {
        return (+price).toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    }
};