var productImage = null;

function InitEvent() {
    $("#createProduct [Name='Image']").on("change", e => {
        ChangeImage(e);
    })
}
InitEvent();

function ChangeImage(e) {
    const formData = GetImageFormData(e);
    

    uploadImage(formData).then(res => {
        if (res.success) {
            productImage = res.data;
        }
    }).catch(err => {
    })
}

function ProductCreate() {
    
    const productCreate = $("#createProduct");
    if (!productCreate) {
        return;
    }
    const productData = {
        Name: $("#createProduct [Name='Name']").val(),
        UnitPrice: $("#createProduct [Name='UnitPrice']").val(),
        Unit: $("#createProduct [Name='Unit']").val(),
        ProductCategoryId: $("#createProduct [Name='ProductCategoryId']").val(),
        Ram: $("#createProduct [Name='Ram']").val(),
        Cpu: $("#createProduct [Name='Cpu']").val(),
        Screen: $("#createProduct [Name='Screen']").val(),
        Pin: $("#createProduct [Name='Pin']").val(),
        WarrantyTime: $("#createProduct [Name='WarrantyTime']").val(),
        LunchTime: $("#createProduct [Name='LunchTime']").val(),
        PositionId: $("#createProduct [Name='PositionId']").val(),
        Quantity: $("#createProduct [Name='Quantity']").val(),
        Image: productImage,
        Origin: $("#createProduct [Name='Origin']").val(),
    }
    baseCreate('/Product/SaveProduct', productData).then(res => {
        if (res.success) {
            window.location.href = '/Product';
        }
    })
}


function GetImageFormData(e) {
    const path = $("#createProduct [Name='Image']").val();

    if (!e || !e.target || !e.target.files || !e.target.files.length || !path) {
        return null;
    }

    let file = e.target.files[0];
    let formData = new FormData();
    formData.append('file', file);
    return formData;
}