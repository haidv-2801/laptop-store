function UpdateReceipt(id) {
    event.preventDefault();
    if ($('#update-receipt-form').valid()) {
        const receiptData = {
            Id: id,
            FirstName: $('#firstName').val(),
            LastName: $('#lastName').val(),
            Phone: $('#phone').val(),
            Email: $('#email').val(),
            Address: $('#address').val(),
        }
        baseUpdate('/Receipt/UpdateReceipt/' + id, receiptData).then(res => {
            if (res.code === ResponseCode.Success) {
                window.location.href = '/Receipt';
            }
        })
    }
}