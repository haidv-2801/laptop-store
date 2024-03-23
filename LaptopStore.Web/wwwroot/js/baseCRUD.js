
function baseGetDataFilterPaging(url, paging) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: url, // Thay 'TenController' bằng tên controller của bạn
            type: 'POST', // Hoặc 'GET' tùy vào cách bạn đã cấu hình action Xoa trong controller
            contentType: 'application/json',
            data: JSON.stringify(paging),
            success: function (result) {
                resolve(result);
            },
            error: function (xhr, status, error) {
                // Xử lý lỗi nếu có
                console.error(error);
                reject(error);
            }
        });
    });

}
function baseDelete(url) {
    console.log(1)
    $.ajax({
        url: url, // Thay 'TenController' bằng tên controller của bạn
        type: 'DELETE', // Hoặc 'GET' tùy vào cách bạn đã cấu hình action Xoa trong controller
        success: function (result) {
            // Xử lý kết quả sau khi xóa, nếu cần
            alert('Xóa thành công');
            // Cập nhật lại giao diện nếu cần
            window.location.reload();
        },
        error: function (xhr, status, error) {
            // Xử lý lỗi nếu có
            console.error(error);
        }
    });
}
function baseCreate(url, data) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: url, // Thay 'TenController' bằng tên controller của bạn
            type: 'POST', // Hoặc 'GET' tùy vào cách bạn đã cấu hình action Xoa trong controller
            contentType: 'application/json',
            data: JSON.stringify(data),
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
function baseUpdate(url, data) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: url, // Thay 'TenController' bằng tên controller của bạn
            type: 'PUT', // Hoặc 'GET' tùy vào cách bạn đã cấu hình action Xoa trong controller
            contentType: 'application/json',
            data: JSON.stringify(data),
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

