$(document).ready(function () {
    GetProductsReceipt();
})

function GetProductsReceipt(id) {
    fetch("/Receipt/GetProductsReceipt").then(res => res.json()).then(res => {
        if (res.success) {
            const data = [];
            renderTableReceiptProduct(data);
        }
    }).catch(err => {
        renderTableReceiptProduct([]);
    });
}