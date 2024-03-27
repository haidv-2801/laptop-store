var selectedItems = []; // Mảng lưu trữ các bản ghi được chọn
var availableItems = []; // Mảng bản ghi được chọn trước đó
$(document).ready(function () {

    // Xử lý chọn từng bản ghi
    $(document).on('change', '.checkbox-item',function () {
        var isChecked = $(this).prop('checked');
        var item = JSON.parse(decodeURIComponent($(this).data('item')));

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
                selectedItems.push(JSON.parse(decodeURIComponent(item)));
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
        const thisId = $(this).data('id')
        let currentItem = selectedItems.find(e => e.id === thisId)
        currentItem.selectedUnitPrice = unitPrice
        currentItem.selectedQuantity = quantity
        currentItem.selectedAmount = amount
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

    //Kiểm tra những bản ghi được thêm mới hoặc xóa đi
    let newSelecteds = [...availableItems]
    $.each(selectedItems, function (index, item) {
        //let selectedItem = JSON.parse(decodeURIComponent(item))
        let selectedItem = item
        if (!availableItems.some(e => e.id === selectedItem.id)) {
            newSelecteds.push(selectedItem)
        }
    })

    $.each(availableItems, function (index, item) {
        //let selectedItem = JSON.parse(decodeURIComponent(item))
        let selectedItem = item
        if (!selectedItems.some(e => e.id === selectedItem.id)) {
            newSelecteds=newSelecteds.filter(e => e.id !== selectedItem.id)
        }
    })

    availableItems = newSelecteds

    $.each(availableItems, function (index, item) {
        //let selectedItem = JSON.parse(decodeURIComponent(item))
        let selectedItem = item
        let imagePath = null;
        if (selectedItem.image) {
            imagePath = `${selectedItem.image}`;
        }

        var newRow = document.createElement('tr');
        newRow.innerHTML = `
                            <td class="pe-auto">${selectedItem.name ?? ""}</td>
                            <td><img class="pe-auto" style="max-height: 100px;" src='${imagePath}'></img></td>
                            <td>
                                <input type='number' class="quantity form-control" data-id="${selectedItem.id}" value="${selectedItem.selectedQuantity}" />
                                <span class="validate-text text-danger"></span>
                            </td>
                            <td>
                                <input type='number' class="unit-price form-control" data-id="${selectedItem.id}" value="${selectedItem.selectedUnitPrice}" />
                                <span class="validate-text text-danger"></span>
                            </td>
                            <td class="amount">${selectedItem.selectedAmount||''}</td>
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
                if (availableItems.length === data?.length) {
                    $('#selectAll').prop('checked', true);
                } else {
                    $('#selectAll').prop('checked', false);

                }
                $.each(data, function (index, item) {
                    let imagePath = null;
                    if (item.image) {
                        imagePath = `${item.image}`;
                    }

                    var newRow = document.createElement('tr');
                    newRow.innerHTML = `
                                        <td><input type="checkbox" class="checkbox-item" data-id="${item.id}" data-item="${encodeURIComponent(JSON.stringify(item))}" ${availableItems.some(e=>e.id === item.id)?"checked":""}/></td>
                                        <td class="pe-auto">${item.name ?? ""}</td>
                                        <img class="pe-auto" style="max-height: 100px;" src='${imagePath}'></img>
                                       `
                    document.getElementById('productList').append(newRow)
                })
            }
        }
    })
}