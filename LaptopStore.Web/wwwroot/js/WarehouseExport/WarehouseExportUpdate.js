var selectedItems = []; // Mảng lưu trữ các bản ghi được chọn
var availableItems = []; // Mảng bản ghi được chọn trước đó
$(document).ready(function () {
    var searchParams = new URLSearchParams(window.location.search);
    var id = searchParams.get('id');
    if (id) { 

    GetProductsExport(id);
    }

    

    // Xử lý chọn từng bản ghi
    $(document).on('change', '.checkbox-item', function () {
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
        let currentItem = availableItems.find(e => e.id === thisId)
        currentItem.selectedUnitPrice = unitPrice
        currentItem.selectedQuantity = quantity
        currentItem.selectedAmount = amount
        CalculateTotal()
    });
});

function GetProductsExport(id) {
    fetch("/WarehouseExport/GetProductsExport/" + id).then(res => {
        return res.json()
    }
    ).then(res => {
        if (res.code == 200) {
            availableItems = res.data.warehouseExportDetails.map(e => {
                e.selectedQuantity = e.quantity
                e.selectedUnitPrice = e.unitPrice
                e.selectedAmount = e.quantity * e.unitPrice
                return e
            })
            selectedItems = availableItems
            renderSelectedProductTable();
        }
    }).catch(err => {
        availableItems = []
        selectedItems = availableItems
        renderSelectedProductTable();
    });
}

function UpdateWarehouseExport() {
    event.preventDefault();
    let data = $('#update-warehouse-export-form').serializeArray();
    if ($('#update-warehouse-export-form').valid()) {
        console.log(JSON.stringify(data))
        let newData = {}
        data.forEach(e => {
            newData[e.name]=e.value
        })
        const customerData = {
            Id: newData.Id,
            ExportTime: $('#exportTime').val(),
            Status: $('#status').val(),
            CustomerId: $('#customer').val(),
            Products: availableItems.map(e => {
                return {
                    Id: e.id,
                    UnitPrice: e.selectedUnitPrice,
                    Quantity: e.selectedQuantity
                }
            })
        }
        baseUpdate('/WarehouseExport/UpdateWarehouseExport/' + newData.Id, customerData).then(res => {
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

function createNewSelectedProduct() {
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
            newSelecteds = newSelecteds.filter(e => e.id !== selectedItem.id)
        }
    })

    availableItems = newSelecteds
}

function handleDeleteOneSelectedProduct(id) {
    if (id) {
        availableItems = availableItems.filter(f => f.id !== id);
        renderSelectedProductTable();
    }
}
function renderSelectedProductTable() {
    document.getElementById('productSelectedList').innerHTML = ""
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
                            <td class="pe-auto"><img class="pe-auto" style="max-height: 100px;" src='${imagePath}'></img></td>
                            <td class="pe-auto">
                                <input type='number' class="quantity form-control" data-id="${selectedItem.id}" value="${selectedItem.selectedQuantity ?? 0}" />
                                <span class="validate-text text-danger"></span>
                            </td>
                            <td class="pe-auto">
                                <input type='number' class="unit-price form-control" data-id="${selectedItem.id}" value="${selectedItem.selectedUnitPrice ?? 0}" />
                                <span class="validate-text text-danger"></span>
                            </td>
                            <td class="pe-auto amount">${selectedItem.selectedAmount ?? 0}</td>
                            <td class="pe-auto">
                                <div onclick="handleDeleteOneSelectedProduct('${selectedItem.id}')" class="btn btn-outline-danger btn-sm">Xóa</div>
                            </td>
                            `
        document.getElementById('productSelectedList').append(newRow)
    })
    var totalRow = document.createElement('tr');
    totalRow.innerHTML = `<tr>
                            <td class="pe-auto"><b>Tổng</b></td>
                            <td></td>
                            <td class="pe-auto total-quantity"><b>0</b></td>
                            <td class="pe-auto"></td>
                            <td class="pe-auto total-amount"><b>0</b></td>
                            <td class="pe-auto">
                            </td>
                        </tr>
                        `
    document.getElementById('productSelectedList').append(totalRow);
    CalculateTotal()
}

function CalculateTotal() {
    let totalQuan = 0,
        totalCurrency = 0;

    [...$('#productSelectedList tr')].forEach((item, index) => {
        const quan = parseFloat($(item).find(".quantity").val() || '0');
        const price = parseFloat($(item).find(".unit-price").val() || '0') * quan;
        totalQuan += quan;
        totalCurrency += price;
    });

    $('#productSelectedList').find(".total-quantity b").text(`${totalQuan}`);
    $('#productSelectedList').find(".total-amount b").text(`${formatNumber(totalCurrency)} VNĐ`);
}


function SaveProductSelected() {
    $(modalAddProduct).modal('hide')

    createNewSelectedProduct()

    renderSelectedProductTable()
}

function renderTableWarehouseExportProductModal(data = []) {
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
                                        <td><input type="checkbox" class="checkbox-item" data-id="${item.id}" data-item="${encodeURIComponent(JSON.stringify(item))}" ${availableItems.some(e => e.id === item.id) ? "checked" : ""}/></td>
                                        <td class="pe-auto">${item.name ?? ""}</td>
                                        <img class="pe-auto" style="max-height: 100px;" src='${imagePath}'></img>
                                       `
                document.getElementById('productList').append(newRow)
            })
        }
    }
}
function OpenModalAddProduct() {
    $('#modalAddProduct').modal('show');

    fetch("/Product/ListAllProduct").then(res => res.json()).then(data => {
        renderTableWarehouseExportProductModal(data)
    })
}