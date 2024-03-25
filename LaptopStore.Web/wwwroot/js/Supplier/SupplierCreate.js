function CreateSupplier() {
    event.preventDefault();
    if ($('#create-supplier-form').valid()) {
        const supplierData = {
            Name: $('#supplierName').val(),
            ContactName: $('#contactName').val(),
            Phone: $('#phone').val(),
            Email: $('#email').val(),
        }
        baseCreate('/Supplier/SaveSupplier', supplierData).then(res => {
            if (res.code === ResponseCode.Success) {
                window.location.href = '/Supplier';
            }
        })
    }
}