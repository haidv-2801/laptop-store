function CreatePosition() {
    let acreageValue = $('#acreage').val()
    let quantityValue = $('#productQuantity').val()
    const positionData = {
        Name: $('#positionName').val(),
        Acreage: (acreageValue && !isNaN(acreageValue)) ? Number(acreageValue) : 0,
        Quantity: (quantityValue && !isNaN(quantityValue)) ? Number(quantityValue) : null
    }
    baseCreate('/Position/SavePosition', positionData).then(res => {
        window.location.href = '/Position';
    })
    return false
} 