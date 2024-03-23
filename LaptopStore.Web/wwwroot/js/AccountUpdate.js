function UpdateAccount(id) {
    const accountData = {
        Id: id,
        Username: $('#username').val(),
        Password: $('#password').val(),
        FullName: $('#fullname').val(),
        Gender: $('#gender').val() ? Number($('#gender').val()) : null,
        AccountType: $('#accountType').val() ? Number($('#accountType').val()) : null,
        Address: $('#address').val(),
    }
    baseUpdate('/Account/UpdateAccount/'+id, accountData).then(res => {
        window.location.href = '/Account';
    })
}