    var selectedItems = []; // Mảng lưu trữ các bản ghi được chọn
$(document).ready(function () {

    // Xử lý chọn từng bản ghi
    $('.checkbox-item').change(function () {
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
    /*$('#btn-save-product').click(function () {
        // Gửi danh sách các bản ghi được chọn đến server
        // Ví dụ: $.post('/Controller/Action', { selectedItems: selectedItems });
    });*/
    // Xử lý tính thành tiền khi nhập đơn giá và số lượng
    $(document).on('input', '.quantity, .unit-price', function () {
        var unitPrice = parseFloat($(this).closest('tr').find('.unit-price').val()) || 0;
        var quantity = parseInt($(this).closest('tr').find('.quantity').val()) || 0;
        var amount = unitPrice * quantity;
        $(this).closest('tr').find('.amount').text(amount);
    });
});

function CreateWarehouseExport() {
    event.preventDefault();
    if ($('#create-warehouseExport-form').valid()) {
        const customerData = {
            FirstName: $('#firstName').val(),
            LastName: $('#lastName').val(),
            Phone: $('#phone').val(),
            Email: $('#email').val(),
            Address: $('#address').val(),
        }
        baseCreate('/WarehouseExport/SaveWarehouseExport', customerData).then(res => {
            if (res.code === ResponseCode.Success) {
                window.location.href = '/WarehouseExport';
            }
        })
    }
}

function CloseAddProductModal() {
    selectedItems = []
    $(modalAddProduct).modal('hide')
}

function SaveProductSelected() {
    $(modalAddProduct).modal('hide')
    document.getElementById('productSelectedList').innerHTML = ""
    $.each(selectedItems, function (index, item) {
        let selectedItem = JSON.parse(decodeURIComponent(item))
        let imagePath = null;
        if (selectedItem.image) {
            imagePath = `${selectedItem.image}`;
        }

        var newRow = document.createElement('tr');
        newRow.innerHTML = `
                            <td class="pe-auto">${selectedItem.name ?? ""}</td>
                            <td><img class="pe-auto" style="max-height: 100px;" src='${imagePath}'></img></td>
                            <td>
                                <input type='number' class="quantity form-control" data-id="${selectedItem.Id}" />
                                <span class="validate-text text-danger"></span>
                            </td>
                            <td>
                                <input type='number' class="unit-price form-control" data-id="${selectedItem.Id}" />
                                <span class="validate-text text-danger"></span>
                            </td>
                            <td class="amount"></td>
                            `
        document.getElementById('productSelectedList').append(newRow)
})
}

function OpenModalAddProduct() {
    $('#modalAddProduct').modal('show');

    fetch("/Product/ListAllProduct").then(res => res.json()).then(data => {
        if (data) {
            $("#productList").html("")
            if (data?.length > 0) {
                $.each(data, function (index, item) {
                    let imagePath = null;
                    if (item.image) {
                        imagePath = `${item.image}`;
                    }

                    var newRow = document.createElement('tr');
                    newRow.innerHTML = `
                                        <td><input type="checkbox" class="checkbox-item" data-id="${item.id}" data-item="${encodeURIComponent(JSON.stringify(item))}" /></td>
                                        <td class="pe-auto">${item.name ?? ""}</td>
                                        <img class="pe-auto" style="max-height: 100px;" src='${imagePath}'></img>
                                       `
                    document.getElementById('productList').append(newRow)
                })
            }
        }
    })
}