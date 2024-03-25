function CreateCustomer() {
    event.preventDefault();
    if ($('#create-customer-form').valid()) {
        const customerData = {
            FirstName: $('#firstName').val(),
            LastName: $('#lastName').val(),
            Phone: $('#phone').val(),
            Email: $('#email').val(),
            Address: $('#address').val(),
        }
        baseCreate('/Customer/SaveCustomer', customerData).then(res => {
            if (res.code === ResponseCode.Success) {
                window.location.href = '/Customer';
            }
        })
    }
}