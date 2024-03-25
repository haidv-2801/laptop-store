function UpdateCustomer(id) {
    event.preventDefault();
    if ($('#update-customer-form').valid()) {
        const customerData = {
            Id: id,
            FirstName: $('#firstName').val(),
            LastName: $('#lastName').val(),
            Phone: $('#phone').val(),
            Email: $('#email').val(),
            Address: $('#address').val(),
        }
        baseUpdate('/Customer/UpdateCustomer/' + id, customerData).then(res => {
            if (res.code === ResponseCode.Success) {
                window.location.href = '/Customer';
            }
        })
    }
}