var currentPage = 1;
var pageTotal = 1
var size = 5
var search = '';
var deleteId = null
function DeletePosition() {
    if (deleteId) {
        baseDelete('/Position/DeletePosition/' + deleteId).then(res => {
            if (res.code === ResponseCode.Success) {
                currentPage = 1
                getDataByPaging()
            }
            $('#deleteConfirm').modal('hide');
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
    baseGetDataFilterPaging('/Position/GetPositionPaging', paging).then(res => {
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
                            <span class="sr-only">Trước</span>
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
                            <span class="sr-only">Sau</span>
                        </a>
                    </li>
                    `
}

function renderDataList(data) {
    document.getElementById('table-body').innerHTML = ""
    if (data?.length > 0) {
        $.each(data, function (index, item) {

            let updateUrl = 'Position/Update?id=' + item.id;
            let detailUrl = 'Position/Detail?id=' + item.id;
            var newRow = document.createElement('tr');
            newRow.innerHTML = `
                                        <td>${item.name}</td>
                                        <td>${item.acreage}</td>
                                        <td>${item.quantity}</td>
                                        <td>
                                            <a href="${updateUrl}" class="btn btn-outline-warning btn-sm">Sửa</a>
                                            <div onclick="handleViewDetail('${item.id}')" class="btn btn-outline-info btn-sm">Chi tiết</div>
                                            <div onclick="handleDelete('${item.id}')" class="btn btn-outline-danger btn-sm">Xóa</div>
                                        </td>`
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

function handleViewDetail(id) {
    baseGetPartialView('Position/GetDetail/' + id).then(res => {
        $('#positionDetailBody').html(res);
        $('#positionDetail').modal('show');
    })
}

// Xử lý sự kiện click xác nhận xóa
$('#btn-delete-confirm').on('click', function () {
    DeletePosition()
});

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