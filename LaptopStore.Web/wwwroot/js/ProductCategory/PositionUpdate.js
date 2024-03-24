function UpdatePosition(id) {
    event.preventDefault();
    if ($('#update-position-form').valid()) {
        const positionData = {
            Id: id,
            Name: $('#positionName').val(),
            Acreage: $('#acreage').val(),
            Quantity: $('#productQuantity').val()
        }
        baseCreate('/Position/UpdatePosition/' + id, positionData).then(res => {
            window.location.href = '/Position';
        })
    }
}