(function ($) {
    function OrdersPage() {
        var orderListElm = $('#orders');
        var rowsPerPage = 10;

        GetOrders(1, rowsPerPage, function (data) {
            RenderUI(data, 1);
        });

        orderListElm.on('click', '.page-link', function () {
            var self = $(this);
            var page = parseInt(self.attr('data-page'));

            GetOrders(page, rowsPerPage, function (data) {
                RenderUI(data, page);
            });
        });

        function GetOrders(pageIndex, pageSize, callback) {
            $.ajax({
                url: '/orders/api/my-orders',
                data: {
                    pageIndex: pageIndex,
                    pageSize: pageSize
                },
                success: function (data) {
                    if (callback instanceof Function) {
                        callback(data);
                    }
                }
            });
        }

        function RenderUI(data, currentPage) {
            if (data?.list?.length) {
                var itemsHtml = '';
                data.list.forEach(order => {
                    itemsHtml += RenderOrderRow(order);
                });

                var html =
                    `<table class="table">
                    <thead>
                        <tr>
                            <th scope="col">Number</th>
                            <th scope="col">Order Date</th>
                            <th scope="col" class="text-right">Total price</th>
                            <th scope="col" class="text-right">Status</th>
                        </tr>
                    </thead>
                    <tbody>${itemsHtml}</tbody>
                </table>`;

                html += RenderPagination(data, currentPage);

                orderListElm.html(html);
            }
            else {
                orderListElm.html('<div class="py-5 bold">You have no orders at the moment! <a href="/" class="font-italic font-weight-bold color-brandred">Let\'s go shopping!</a></div>');
            }
        }

        function RenderOrderRow(order) {
            var html =
                `<tr>
                    <th><a href="/orders/${order.id}">#${order.id}</a></th>
                    <td class="order-date-cell">${new Date(order.createdAt).toLocaleDateString()}</td>
                    <td class="price-cell color-brandred">$${priceUtils.formatPrice(order.amount)}</td>
                    <td class="status-cell status-${order.status}">${order.statusText}</td>
                </tr>`;

            return html;
        }

        function RenderPagination(data, currentPage) {
            var startPage;
            if (currentPage === 1) {
                startPage = 1;
            }
            else if (currentPage === data.totalPages) {
                startPage = currentPage - 2;
            }
            else {
                startPage = currentPage - 1;
            }

            var pageNumbersHtml = '';
            for (var i = startPage; i <= startPage + 2; i++) {
                pageNumbersHtml +=
                    `<li class="page-item ${(i === currentPage ? "active" : "")}">
                        <span class="page-link" data-page="${i}">${i}</span>
                    </li>`;
            }

            var html =
                `<ul class="pagination justify-content-center">
                    <li class="page-item">
                        <span class="page-link" aria-label="First" data-page="1">
                            <span aria-hidden="true">«</span>
                        <span class="sr-only">First</span>
                      </span>
                    </li>
                    ${pageNumbersHtml}
                    <li class="page-item">
                        <span class="page-link" aria-label="Next" data-page="${data.totalPages}">
                            <span aria-hidden="true">»</span>
                            <span class="sr-only">Last</span>
                        </span>
                    </li>
                </ul>`;

            return html;
        }
    }

    $(document).ready(function () {
        new OrdersPage();
    });
})(jQuery);

