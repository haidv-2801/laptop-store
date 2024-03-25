function UpdateSupplier(id) {
    event.preventDefault();
    if ($('#update-supplier-form').valid()) {
        const supplierData = {
            Id: id,
            Name: $('#supplierName').val(),
            ContactName: $('#contactName').val(),
            Phone: $('#phone').val(),
            Email: $('#email').val(),
        }
        baseUpdate('/Supplier/UpdateSupplier/' + id, supplierData).then(res => {
            if (res.code === ResponseCode.Success) {
                window.location.href = '/Supplier';
            }
        })
    }
}