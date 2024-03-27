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
                                        <td class="pe-auto">${item.name ?? ""}</td>
                                        <img class="pe-auto" style="max-height: 100px;" src='${imagePath}'></img>
                                       `
                    document.getElementById('productList').append(newRow)
                })
            }
        }
    })
}