var selectedItems = []; // Mảng lưu trữ các bản ghi được chọn
var receiptProducts = [];

$(document).ready(function () {

    // Xử lý chọn từng bản ghi

    $(document).on('change', '.checkbox-item', function () { 
        var isChecked = $(this).prop('checked');
        var item = $(this).data('item');

        // Nếu checkbox được chọn, thêm itemId vào danh sách
        if (isChecked) {
            selectedItems.push(item);
        } else {
            // Nếu checkbox bị bỏ chọn, loại bỏ itemId khỏi danh sách
            selectedItems = selectedItems.filter(selectedItem => selectedItem.id !== item.id);
        }
    });

    // Xử lý chọn tất cả
    $('#selectAll').change(function () {
        var isChecked = $(this).prop('checked');
        $('.checkbox-item').prop('checked', isChecked);

        // Nếu checkbox được chọn, thêm tất cả các itemId vào danh sách
        if (isChecked) {
            $('.checkbox-item').each(function () {
                var item = $(this).data('item');
                selectedItems.push(item);
            });
        } else {
            // Nếu checkbox bị bỏ chọn, xóa tất cả các itemId khỏi danh sách
            selectedItems = [];
        }
    });

    // Xử lý khi nhấn nút Submit
    // Xử lý tính thành tiền khi nhập đơn giá và số lượng
    $(document).on('input', '.quantity, .unit-price', function () {
        var id = $(this).closest("tr").attr("id");
        var unitPrice = parseFloat($(this).closest('tr').find('.unit-price').val()) || 0;
        var quantity = parseInt($(this).closest('tr').find('.quantity').val()) || 0;
        var amount = unitPrice * quantity;
        $(this).closest('tr').find('.amount').text(amount);
        if (id) {
            receiptProducts = receiptProducts.map(f => ({ ...f, quantity: quantity, unitPrice: unitPrice  }))
        }
        Calculate();
    });
});

function CloseAddProductModal() {
    selectedItems = []
    $(modalAddProduct).modal('hide')
}

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

function CreateReceipt() {
    event.preventDefault();
    if ($('#create-receipt-form').valid()) {
        const customerData = {
            FirstName: $('#firstName').val(),
            LastName: $('#lastName').val(),
            Phone: $('#phone').val(),
            Email: $('#email').val(),
            Address: $('#address').val(),
        }
        baseCreate('/Receipt/SaveReceipt', customerData).then(res => {
            if (res.code === ResponseCode.Success) {
                window.location.href = '/Receipt';
            }
        })
    }
}

function handleDeleteReceiptProduct(id) {
    if (id) {
        receiptProducts = receiptProducts.filter(f => f.id !== id);
        renderTableReceiptProduct(receiptProducts);
    }
}

function renderTableReceiptProduct(data = []) {
    $("#receiptProducts tbody").html("");
    $.each(data, (index,item) => {
        let imagePath = null;
        if (item.image) {
            imagePath = `${item.image}`;
        }
        var newRow = document.createElement('tr');
        newRow.setAttribute("id", item.id);
        newRow.innerHTML = `<td class="pe-auto">${item.name ?? ""}</td>
                                        <td><img class="pe-auto" style="max-height: 100px;" src='${imagePath}'></img></td>
                                        <td class="pe-auto"><input type="number" class="quantity" style="width: 100px;" value='0'></input></td>
                                        <td class="pe-auto"><input type="number" class="unit-price" style="width: 100px;" value='0'></input></td>
                                        <td class="pe-auto amount">0</td>
                                        <td class="pe-auto">
                                            <div onclick="handleDeleteReceiptProduct('${item.id}')" class="btn btn-outline-danger btn-sm">Xóa</div>
                                        </td>
                                       `
        $("#receiptProducts tbody").append(newRow);
    })
    $("#receiptProducts tbody").append(`<tr><td class="pe-auto"><b>Tổng</b></td>
                                        <td></td>
                                        <td class="pe-auto total-quantity"><b>0</b></td>
                                        <td class="pe-auto"></td>
                                        <td class="pe-auto total-amount"><b>0</b></td>
                                        <td class="pe-auto">
                                        </td></tr>
                                       `);

    Calculate();
    if (receiptProducts && receiptProducts.length) {
        $("#receiptProductsWrapper").removeClass("d-none");
    } else {
        $("#receiptProductsWrapper").addClass("d-none");
    }

}

function renderTableReceiptProductModal(data = []) {
    $("#receiptProductsModal tbody").html("");
    $.each(data, (index, item) => {
        let imagePath = null;
        if (item.image) {
            imagePath = `${item.image}`;
        }
        var newRow = document.createElement('tr');
        //newRow.addEventListener("dblclick", (e) => handleClickAddProductToReceipt(e, item))
        newRow.setAttribute("id", item.id);
        newRow.innerHTML = `
                                <td><input type="checkbox" class="checkbox-item" data-id="${item.id}" data-item="${encodeURIComponent(JSON.stringify(item))}" /></td>
                                <td class="pe-auto">${item.name ?? ""}</td>
                                        <td><img class="pe-auto" style="max-height: 100px;" src='${imagePath}'></img></td>
                                       `
        $("#receiptProductsModal tbody").append(newRow);
    })
}

function handleClickAddProductToReceipt(e, item) {
    if (selectedItems && selectedItems.length) {
        let datas = [];
        selectedItems.forEach((item, index) => {
            datas.push(JSON.parse(decodeURIComponent(item)))
        })
        receiptProducts = [...receiptProducts, ...datas];
        renderTableReceiptProduct(receiptProducts);
        selectedItems = [];
        $('#modalAddProduct').modal('hide');
    }

}


function OpenModalAddProduct() {
    $('#modalAddProduct').modal('show');

    fetch("/Product/ListAllProduct").then(res => res.json()).then(data => {
        if (data) {
            if (data?.length > 0) {
                renderTableReceiptProductModal(data);
            }
        }
    })
}


function CreateReceipt() {
    event.preventDefault();
    if ($('#create-receipt-form').valid()) {
        const customerData = {
            ImportTime: $('#importTime').val(),
            Status: $('#status').val(),
            SupplierId: $('#supplier').val(),
            Products: receiptProducts.map(e => {
                return {
                    Id: e.id,
                    UnitPrice: e.unitPrice,
                    Quantity: e.quantity
                }
            })
        }
        baseCreate('/Receipt/SaveReceipt', customerData).then(res => {
            if (res.code === ResponseCode.Success) {
                window.location.href = '/Receipt';
            }
        })
    }
}