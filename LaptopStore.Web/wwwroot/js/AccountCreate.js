function CreateAccount() {
    const accountData = {
        Username: $('#username').val(),
        Password: $('#password').val(),
        FullName: $('#fullname').val(),
        Gender: $('#gender').val() ? Number($('#gender').val()) : null,
        AccountType: $('#accountType').val() ? Number($('#accountType').val()) : null,
        Address: $('#address').val(),
    }
    baseCreate('/Account/SaveAccount', accountData).then(res => {
        window.location.href = '/Account';
    })
    return false
}

function Login() {
    const accountData = {
        Username: $('#username').val(),
        Password: $('#password').val(),
    }
    baseCreate('/Auth/Login', accountData).then(res => {
        debugger
        if (!res.success) {
            $('.errorLogin').text(res.message ?? "Có Lỗi xảy ra.");
        } else {
            window.location.href = "/Account";
        }
    }).catch(err => {
    })
}