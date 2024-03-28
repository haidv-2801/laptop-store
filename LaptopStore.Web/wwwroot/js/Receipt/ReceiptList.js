var currentPage = 1;
var pageTotal = 1
var size = 5
var search = '';
var deleteId = null
function DeleteReceipt() {
    if (deleteId) {
        baseDelete('/Receipt/DeleteReceipt/' + deleteId).then(res => {
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
        SearchField: 'Username'
    }
    // Gọi hàm JavaScript của bạn ở đây
    baseGetDataFilterPaging('/Receipt/GetReceiptPaging', paging).then(res => {
        if (res.code === 200) {
            const data = res.data?.data
            debugger
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

            let updateUrl = 'Receipt/Update?id=' + item.id;
            let detailUrl = 'Receipt/Detail?id=' + item.id;
            let statusText = 'Mới';
            if (item.status == '0') {
                statusText = 'Mới';
            }
            else if (item.status == '1') {
                statusText = 'Hoàn thành';
            }
            else {
                statusText = 'Không xác định';
            }
            let edit = item.status == '0' ? `<a href="${updateUrl}" class="btn btn-outline-warning btn-sm">Sửa</a>` : ``;
            let del = item.status == '0' ? `<div onclick="handleDelete('${item.id}')" class="btn btn-outline-danger btn-sm">Xóa</div>` : ``;

            var newRow = document.createElement('tr');
            newRow.innerHTML = `
                                        <td>${item.importTime}</td>
                                        <td>${statusText}</td>
                                        <td>${item.username}</td>
                                        <td>
                                            ${edit}
                                            <div onclick="handleViewDetail('${item.id}')" class="btn btn-outline-info btn-sm">Chi tiết</div>
                                             ${del}
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
    baseGetPartialView('Receipt/GetDetail/' + id).then(res => {
        $('#customerDetailBody').html(res);
        $('#customerDetail').modal('show');
    })
}

// Xử lý sự kiện click xác nhận xóa
$('#btn-delete-confirm').on('click', function () {
    DeleteReceipt()
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