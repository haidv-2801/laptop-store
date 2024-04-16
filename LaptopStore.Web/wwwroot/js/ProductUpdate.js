var productImage = null;

function InitEvent() {
    $("#updateProduct [Name='Image']").on("change", e => {
        ChangeImage(e);
    })
}
InitEvent();


function UpdateProduct(id) {
    event.preventDefault();
    if ($('#update-product-form').valid()) {
        const productData = {
            Name: $("#updateProduct [Name='Name']").val(),
            UnitPrice: $("#updateProduct [Name='UnitPrice']").val(),
            Unit: $("#updateProduct [Name='Unit']").val(),
            ProductCategoryId: $("#updateProduct [Name='ProductCategoryId']").val(),
            Ram: $("#updateProduct [Name='Ram']").val(),
            Cpu: $("#updateProduct [Name='Cpu']").val(),
            Screen: $("#updateProduct [Name='Screen']").val(),
            Pin: $("#updateProduct [Name='Pin']").val(),
            WarrantyTime: $("#updateProduct [Name='WarrantyTime']").val(),
            LunchTime: $("#updateProduct [Name='LunchTime']").val(),
            PositionId: $("#updateProduct [Name='PositionId']").val(),
            Quantity: $("#updateProduct [Name='Quantity']").val(),
            Image: $('#productImage img').attr("src"),
            Origin: $("#updateProduct [Name='Origin']").val(),
        }
        baseUpdate('/Product/UpdateProduct/' + id, productData).then(res => {
            window.location.href = '/Product';
        })
    }
}


function ChangeImage(e) {
    const formData = GetImageFormData(e);


    uploadImage(formData).then(res => {
        if (res.success) {
            productImage = res.data;
            $('#productImage img').attr("src", productImage);
        }
    }).catch(err => {
    })
}

function GetImageFormData(e) {
    const path = $("#updateProduct [Name='Image']").val();

    if (!e || !e.target || !e.target.files || !e.target.files.length || !path) {
        return null;
    }

    let file = e.target.files[0];
    let formData = new FormData();
    formData.append('file', file);
    return formData;
}