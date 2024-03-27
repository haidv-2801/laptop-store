function UpdateWarehouseExport(id) {
    event.preventDefault();
    if ($('#update-warehouseExport-form').valid()) {
        const warehouseExportData = {
            Id: id,
            FirstName: $('#firstName').val(),
            LastName: $('#lastName').val(),
            Phone: $('#phone').val(),
            Email: $('#email').val(),
            Address: $('#address').val(),
        }
        baseUpdate('/WarehouseExport/UpdateWarehouseExport/' + id, warehouseExportData).then(res => {
            if (res.code === ResponseCode.Success) {
                window.location.href = '/WarehouseExport';
            }
        })
    }
}