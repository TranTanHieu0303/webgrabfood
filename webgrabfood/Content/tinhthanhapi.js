
$(document).ready(function () {
    var prov = document.getElementById('province');
    window.onload = function () {
        /*prov.addEventListener('click', function () {*/
        $.ajax({
            url: "https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/province",
            headers: {
                'token': 'e4ba402d-b785-11eb-8080-c61f6a6501ca',
                'Content-Type': 'application/json'
            },
            method: 'GET',
            dataType: 'json',
            success: function (response) {
                console.log('succes: ');
                console.log(response.data);
                var str = "<option >Tỉnh/Thành Phố</option>";
                for (var i = 0; i < response.data.length; i++) {
                    console.log(response.data[i].ProvinceName);
                    str = str + "<option class='provinceId' data-province='" + response.data[i].ProvinceID + "'>" + response.data[i].ProvinceName + "</option>"
                }
                prov.innerHTML = str;

            }


        })
    }
    }, false)
function changeFunc() {
    let selectbox = document.getElementById('province');
    var selectValue = selectbox.options[selectbox.selectedIndex].getAttribute('data-province');
    var distric = document.getElementById('district');
    $.ajax({
        url: " https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/district",
        headers: {
            'token': 'e4ba402d-b785-11eb-8080-c61f6a6501ca',
            'Content-Type': 'application/json'
        },
        method: 'GET',
        dataType: 'json',
        success: function (response) {
            var str = "<option >Quận/Huyện</option>";
            for (var i = 0; i < response.data.length; i++) {
                if (response.data[i].ProvinceID == selectValue)
                    str = str + "<option class='DistrictID' data-district='" + response.data[i].DistrictID + "'>" + response.data[i].DistrictName + "</option>"

            }
            distric.innerHTML = str;
        }
    })

};
function changeFuncDictrict() {
    let selectbox = document.getElementById('district');
    var selectValue = selectbox.options[selectbox.selectedIndex].getAttribute('data-district');
    var ward = document.getElementById('ward');
    $.ajax({
        url: "https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/ward?district_id=" + selectValue,
        headers: {
            'token': 'e4ba402d-b785-11eb-8080-c61f6a6501ca',
            'Content-Type': 'application/json'
        },
        method: 'GET',
        dataType: 'json',
        success: function (response) {
            var str = "<option >Phường/Xã</option>";
            for (var i = 0; i < response.data.length; i++) {
                //if (response.data[i].DistrictID == selectValue)
                    str = str + "<option class='WardID' data-wart='" + response.data[i].WardCode + "'>" + response.data[i].WardName + "</option>"

            }
            ward.innerHTML = str;
        }
    })

};
function addtaikhoan() {
    //var sdt = document.getElementById('SDT').value;
    //var gmail = document.getElementById('gmail').value;
    //const firebaseConfig = {
    //    apiKey: "AIzaSyCl42UQ0cpTZqV5PJ_djoIQT6zuoPkm6WE",
    //    authDomain: "webgrabfood.firebaseapp.com",
    //    databaseURL: "https://webgrabfood-default-rtdb.firebaseio.com",
    //    projectId: "webgrabfood",
    //    storageBucket: "webgrabfood.appspot.com",
    //    messagingSenderId: "342657322371",
    //    appId: "1:342657322371:web:264ee78e04ae26616a7ab6",
    //    measurementId: "G-VT6KHM6347"
    //};
    //firebase.initializeApp(firebaseConfig);
    //var database = firebase.database();
    //database.ref('KhachHang/'+'3').set({
    //    sdt: "aaa",
    //    gmail: "aaa"
    //});
    var emp = {
        SDT: $('#SDT').val(),
        TenKH: $('#HoTen').val(),
        Gioitinh: $('#gioitinh').val(),
        Gmail: $('#gmail').val(),
        TinhThanh: $('#province').val(),
        QuanHuyen: $('#district').val(),
        PhuongXa: $('#ward').val(),
        SonhaTenDuong: $('#duong').val(),
        Password: $('#Pass').val(),
    };
    $.ajax(
        {
            url: "/KhachHang/DangKy",
            data: JSON.stringify(emp),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                confirm('Thêm thành công');
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }

        });
};
function uploadFile(file) {
    var metadata = {
        contentType: 'image/jpeg',
    };
    var uploadTask = firebase.storage().ref("image" + Date.now()).put(file, metadata);

    // Listen for state changes, errors, and completion of the upload.
    uploadTask.on(
        firebase.storage.TaskEvent.STATE_CHANGED, // or 'state_changed'
        function (snapshot) {
            // Get task progress, including the number of bytes uploaded and the total number of bytes to be uploaded
            var progress = snapshot.bytesTransferred / snapshot.totalBytes * 100;
            progressBar.value = progress;
            console.log('Upload is ' + progress + '% done');
            switch (snapshot.state) {
                case firebase.storage.TaskState.PAUSED: // or 'paused'
                    console.log('Upload is paused');
                    break;
                case firebase.storage.TaskState.RUNNING: // or 'running'
                    console.log('Upload is running');
                    break;
            }
        },
        function (error) {
            // Errors list: https://firebase.google.com/docs/storage/web/handle-errors

            switch (error.code) {
                case 'storage/unauthorized':
                    // User doesn't have permission to access the object
                    tb.title = 'storage/unauthorized';
                    break;

                case 'storage/canceled':
                    // User canceled the upload
                    tb.title = 'storage/canceled';
                    break;

                case 'storage/unknown':
                    tb.title = 'storage/unknown';
                    // Unknown error occurred, inspect error.serverResponse
                    break;
            }
        },
        function () {
            // Upload completed successfully, now we can get the download URL
            var tb = document.getElementById('tb');
            uploadTask.snapshot.ref.getDownloadURL().then(function (downloadURL) {
                console.log('File available at', downloadURL);
                var _img = document.getElementById('photo');
                var newImg = new Image;
                newImg.onload = function () {
                    _img.src = this.src;
                }
                newImg.src = downloadURL;
                tb.value = downloadURL;
                urlhinh = downloadURL;

            });

        }
    )
};