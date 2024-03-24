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
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: url, // Thay 'TenController' bằng tên controller của bạn
            type: 'DELETE', // Hoặc 'GET' tùy vào cách bạn đã cấu hình action Xoa trong controller
            success: function (result) {

                resolve(result);
            },
            error: function (xhr, status, error) {
                // Xử lý lỗi nếu có
                reject(error);
            }
        });
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
function baseGetPartialView(url, data) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: url, // Thay 'TenController' bằng tên controller của bạn
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

function uploadImage(formData) {
    return new Promise(function (resolve, reject) {
        fetch('/api/Storage/Image', {
            method: 'POST',
            body: formData
        })
        .then(response => {
            if (!response.ok) {
                reject(response)
            }
            return response.json();
        })
        .then(data => {
            resolve(data);
        })
        .catch(error => {
            reject(error);
        });
    });
}

