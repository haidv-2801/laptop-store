var receiptProducts = [];

function UpdateReceipt() {
    event.preventDefault();
    debugger
    if ($('#update-receipt-form').valid()) {
        const customerData = {
            ImportTime: $('#importTime').val(),
            Status: $('#status').val(),
            Username: $('#customer').val(),
            Products: BuildProducts()
        }
        let id = $('#update-receipt-form [name="Id"]').val();
        if (!id) return;
        baseUpdate('/Receipt/UpdateReceipt/' + id, customerData).then(res => {
            if (res.code === ResponseCode.Success) {
                window.location.href = '/Receipt';
            }
        })
    }
}

$(document).ready(function () {
    $(document).on('input', '.quantity, .unit-price', function () {
        var id = $(this).closest("tr").attr("id");
        var unitPrice = parseFloat($(this).closest('tr').find('.unit-price').val()) || 0;
        var quantity = parseInt($(this).closest('tr').find('.quantity').val()) || 0;
        var amount = unitPrice * quantity;
        $(this).closest('tr').find('.amount').text(amount);
        Calculate();
    });
});

function Calculate() {

    let totalQuan = 0,
        totalCurrency = 0;

    [...$('#receiptProducts tbody tr')].forEach((item, index) => {
        const quan = parseFloat($(item).find(".quantity").val() || '0');
        const price = parseFloat($(item).find(".unit-price").val() || '0') * quan;
        totalQuan += quan;
        totalCurrency += price;
    });

    $('#receiptProducts tbody').find(".total-quantity b").text(`${totalQuan}`);
    $('#receiptProducts tbody').find(".total-amount b").text(`${totalCurrency} đ`);
}

function BuildProducts() {
    let products = [];
    [...$('#receiptProducts tbody tr')].forEach((item, index) => {
        const quan = parseFloat($(item).find(".quantity").val() || '0');
        const price = parseFloat($(item).find(".unit-price").val() || '0') * quan;
        if ($(item).attr("Id")) {
            products.push({ Id: $(item).attr("Id"), Quantity: quan, UnitPrice: price })
        }
    });
    return products;
}