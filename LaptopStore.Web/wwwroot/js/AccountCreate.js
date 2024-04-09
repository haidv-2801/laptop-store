function CreateAccount() {
    // Ngăn chặn hành động mặc định của form
    event.preventDefault();
    if ($('#create-account-form').valid()) {
        const accountData = {
            Username: $('#username').val(),
            Password: $('#password').val(),
            FullName: $('#fullname').val(),
            Gender: $('#gender').val() ? Number($('#gender').val()) : null,
            AccountType: $('#accountType').val() ? Number($('#accountType').val()) : null,
            Address: $('#address').val(),
            IsDeleted: $('#isDeleted').val() && $('#isDeleted').val() == "true" ? true : false,
        }
        baseCreate('/Account/SaveAccount', accountData).then(res => {
            console.log(res)
            if (res.code === ResponseCode.Success) {
                window.location.href = '/Account';
            } else {
                notifyStyle = 'danger'
                $('#notifyToast').addClass(`bg-${notifyStyle}`)
                $('#notifyToastBody').html(res.message);
                $('#notifyToast').toast('show');
            }
        })
    }
}

function Login() {
    const accountData = {
        Username: $('#username').val(),
        Password: $('#password').val(),
    }
    baseCreate('/Auth/Login', accountData).then(res => {
        if (!res.success) {
            $('.errorLogin').text(res.message ?? "Có Lỗi xảy ra.");
        } else {
            window.location.href = "/Account";
        }
    }).catch(err => {
    })
}