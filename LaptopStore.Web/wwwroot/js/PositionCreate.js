function CreatePosition() {
    event.preventDefault();
    if ($('#create-position-form').valid()) {
        let acreageValue = $('#acreage').val()
        /*let quantityValue = $('#productQuantity').val()*/
        const positionData = {
            Name: $('#positionName').val(),
            Acreage: (acreageValue && !isNaN(acreageValue)) ? Number(acreageValue) : 0,
            /*Quantity: (quantityValue && !isNaN(quantityValue)) ? Number(quantityValue) : null*/
        }
        baseCreate('/Position/SavePosition', positionData).then(res => {
            if (res.code === ResponseCode.Success) {
                window.location.href = '/Position';
            }
        })
    }
}