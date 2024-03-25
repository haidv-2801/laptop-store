var currentPage = 1;
var pageTotal = 1
var size = 5
var search = '';
var deleteId = null
var clickedId = null
function DeleteProductCategory() {
    if (deleteId) {
        baseDelete('/ProductCategory/DeleteProductCategory/' + deleteId).then(res => {
            currentPage=1
            getDataByPaging()
            $('#deleteConfirm').modal('hide');
            $('#deleteSuccessToast').toast('show');
        }).catch(e => {
            $('#deleteFailedToast').toast('show');

        })
    }
}
$(document).ready(function () {
    getDataByPaging()
});


function getDataByPaging() {
    const paging = {
        Page: currentPage,
        PageSize: size,
        Search: search,
        SearchField: 'Name'
    }
    // Gọi hàm JavaScript của bạn ở đây
    baseGetDataFilterPaging('/ProductCategory/GetProductCategoryPaging', paging).then(res => {
        if (res.code === 200) {
            const data = res.data?.data
            renderDataList(data)
            renderPagination(res.data.total, size)
        }
    }).catch(e => {
        console.log(e)
    })
}

function renderPagination(total, size) {
    const paginationElement = document.getElementById('pagination')
    paginationElement.innerHTML = `
                    <div id='page-total' style="margin-right:16px" className='page-total'>Tổng: ${total}</div>
                    <li class="page-item">
                        <a class="btn-prev page-link" href="#" aria-label="Previous">
                            <span aria-hidden="true">&laquo;</span>
                            <span class="sr-only">Previous</span>
                        </a>
                    </li>
                    `
    let pageQuantity = total % size === 0 ? total / size : (Math.floor(total / size) + 1)
    pageTotal = pageQuantity
    for (let i = 1; i <= pageQuantity; i++) {
        paginationElement.innerHTML += `<li class="page-item ${i === currentPage ? 'active' : ''}"><a class="page-link" href="#" data-page="${i}">${i}</a></li>`
    }
    paginationElement.innerHTML += `
                    <li class="page-item">
                        <a class="btn-next page-link" href="#" aria-label="Next">
                            <span aria-hidden="true">&raquo;</span>
                            <span class="sr-only">Next</span>
                        </a>
                    </li>
                    `
}

function renderDataList(data) {
    document.getElementById('table-body').innerHTML = ""
    if (data?.length > 0) {
        $.each(data, function (index, item) {
            var newRow = document.createElement('tr');
            newRow.innerHTML = `
                                        <td>${item.name}</td>
                                        <td>${item.productQuantity}</td>
                                        <td>
                                            <div onclick="handleOpenUpdateModal('${item.id}')" class="btn btn-outline-warning btn-sm">Sửa</div>
                                            <div onclick="handleDelete('${item.id}')" class="btn btn-outline-danger btn-sm">Xóa</div>
                                        </td>`
            /*<div onclick="handleViewDetail('${item.id}')" class="btn btn-outline-info btn-sm">Chi tiết</div>*/
            document.getElementById('table-body').append(newRow)
        })
    } else {
        document.getElementById('table-body').innerHTML = '<span>Không có dữ liệu</span>'
    }
}

function handleDelete(id) {
    deleteId = id
    $('#deleteConfirm').modal('show');
}

function handleOpenCreateModal() {
    baseGetPartialView('ProductCategory/Create').then(res => {
        $('#productCategoryCreateBody').html(res);
        $('#productCategoryCreate').modal('show');
    })
}
function handleOpenUpdateModal(id) {
    clickedId=id
    baseGetPartialView('ProductCategory/Update/'+id).then(res => {
        $('#productCategoryUpdateBody').html(res);
        $('#productCategoryUpdate').modal('show');
    })
}

function handleViewDetail(id) {
    baseGetPartialView('ProductCategory/Create').then(res => {
        $('#productCategoryDetailBody').html(res);
        $('#productCategoryDetail').modal('show');
    })
}

// Xử lý sự kiện click lưu thông tin danh mục mới
function handleSaveCreateData() {
    // Ngăn chặn hành động mặc định của form
    event.preventDefault();
    if ($('#create-product-category-form').valid()) {
        const categoryData = {
            Name: $('#categoryName').val(),
        }
        baseCreate('/ProductCategory/SaveProductCategory', categoryData).then(res => {
            if (res.code === ResponseCode.Success) {

                $('#productCategoryCreate').modal('hide');
                $('#createSuccessToast').toast('show');
                getDataByPaging()
            } else {
                $('#productCategoryToastBody').html(res.message);
                $('#productCategoryToast').toast('show');

            }
        }).catch(e => {
            $('#createFaildToast').toast('show');

        })
    }
};
// Xử lý sự kiện click lưu thông tin sửa danh mục
function handleSaveUpdateData() {
    // Ngăn chặn hành động mặc định của form
    event.preventDefault();
    if ($('#update-product-category-form').valid()) {
        const categoryData = {
            Id: clickedId,
            Name: $('#categoryName').val(),
        }
        baseUpdate('/ProductCategory/UpdateProductCategory/' + clickedId, categoryData).then(res => {
            if (res.code === ResponseCode.Success) {

                $('#productCategoryUpdate').modal('hide');
                $('#updateSuccessToast').toast('show');
                getDataByPaging()
            } else {
                $('#productCategoryToastBody').html(res.message);
                $('#productCategoryToast').toast('show');

            }
        }).catch(e => {
            $('#updateFaildToast').toast('show');

        })
    }
};

// Xử lý sự kiện click xác nhận xóa
$('#btn-delete-confirm').on('click', function () {
    checkExistsProduct().then(res => {
        if (res) {
            $('#deleteExistsProductToast').toast('show');
            $('#deleteConfirm').modal('hide');

        } else {
            DeleteProductCategory()
        }
    })
});

function checkExistsProduct() {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: 'ProductCategory/CheckExistsProduct/' + deleteId, // Thay 'TenController' bằng tên controller của bạn
            type: 'GET', // Hoặc 'GET' tùy vào cách bạn đã cấu hình action Xoa trong controller
            success: function (result) {
                // Xử lý kết quả sau khi xóa, nếu cần
                resolve(result);
            },
            error: function (xhr, status, error) {
                // Xử lý lỗi nếu có
                reject(error);
            }
        });
    });
}

// Xử lý sự kiện click cho các nút phân trang
$('#pagination').on('click', '.page-link', function () {
    // Lấy số trang từ thuộc tính data-page của nút được click
    var page = $(this).data('page');
    if (page) {
        //set active
        //var pageItem = $(this).closest('.page-item');
        //if (pageItem.length > 0) {
        //    $(pageItem[0]).addClass('active')

        //}

        // Lưu lại trang hiện tại
        currentPage = page;

        // Thực hiện tìm kiếm và phân trang
        //searchAndPaginate('', page);
        getDataByPaging()
    }

});

// Xử lý sự kiện click cho nút Previous
$('#pagination').on('click', '.btn-prev', function () {
    // Giảm số trang hiện tại đi một
    if (currentPage > 1) {
        currentPage--;
        // Thực hiện tìm kiếm và phân trang với trang mới
        //searchAndPaginate('', currentPage);
        getDataByPaging()
    }

});

// Xử lý sự kiện click cho nút Next
$('#pagination').on('click', '.btn-next', function () {
    // Tăng số trang hiện tại lên một
    if (currentPage < pageTotal) {

        currentPage++;

        // Thực hiện tìm kiếm và phân trang với trang mới
        //searchAndPaginate('', currentPage);
        getDataByPaging()
    }
});

// Xử lý sự kiện click cho các nút phân trang
$('#btn-search').on('click', function () {
    // Ngăn chặn form gửi dữ liệu và làm mới trang
    event.preventDefault();
    // Lấy số trang từ thuộc tính data-page của nút được click
    search = $('#search-text').val()
    currentPage = 1

    // Thực hiện tìm kiếm và phân trang
    //searchAndPaginate('', page);
    getDataByPaging()
});