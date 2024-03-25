function UpdatePosition(id) {
    event.preventDefault();
    if ($('#update-position-form').valid()) {
        let acreageValue = $('#acreage').val()
        const positionData = {
            Id: id,
            Name: $('#positionName').val(),
            Acreage: (acreageValue && !isNaN(acreageValue)) ? Number(acreageValue) : 0,
            /*Quantity: $('#productQuantity').val()*/
        }
        baseUpdate('/Position/UpdatePosition/' + id, positionData).then(res => {
            if (res.code === ResponseCode.Success) {
                window.location.href = '/Position';
            }
        })
    }
}