function UpdateProduct(id) {
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
            Image: $("#updateProduct [Name='Image']").val(),
            Origin: $("#updateProduct [Name='Origin']").val(),
        }
        baseUpdate('/Product/UpdateProduct/' + id, productData).then(res => {
            window.location.href = '/Product';
        })
    }
}