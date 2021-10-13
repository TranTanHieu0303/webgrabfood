$(document).ready(function () {
    $('#search').on('keyup', function (event) {
        event.preventDefault();
        /* Act on the event */
        var value = $(this).val().toLowerCase();
        $("#mynamesp .row*").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#openfile").click(function () {
        $("input[id='hinhanh']").click();
    });
    $("#suadiachi").click(function () {
        console.log("a");
        $("#diachi").css("display", "block");
    })
    $("#Luudiachi").click(function () {
        let selectbox1 = document.getElementById('province');
        let selectbox2 = document.getElementById('district');
        let selectbox3 = document.getElementById('ward');
        var tinh = selectbox1.options[selectbox1.selectedIndex].text;
        var huyen = selectbox2.options[selectbox2.selectedIndex].text;
        var xa = selectbox3.options[selectbox3.selectedIndex].text;
        var dia = $("#duongCH").val();
        if (tinh != "Tỉnh/Thành Phố" && huyen != "Quận/Huyện" && xa != "Phường/Xã") {
            $("#diac").html("<p> Địa Chỉ : " + dia + ", " + xa + ", " + huyen + ", " + tinh + "</p>");
            console.log("Địa Chỉ : " + dia + ", " + xa + ", " + huyen + ", " + tinh);
        }
        $("#diachi").css("display", "none");
    })
});
function Luudiachi() {
    let selectbox1 = document.getElementById('province');
    let selectbox2 = document.getElementById('district');
    let selectbox3 = document.getElementById('ward');
    var tinh = selectbox1.val();
    var huyen = selectbox2.val();
    var xa = selectbox3.val();
    var dia = $("#duongCH").val();
    $("#diac").innerHTML = "Địa Chỉ : " + dia + ", " + xa + ", " + huyen + ", " + tinh;
}
function Add() {
    var emp = {
        mName: $('#tenloai').val(),
    };
    $.ajax(
        {
            url: "/CuaHang/Themmenu",
            data: JSON.stringify(emp),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result==1)
                    confirm('Thêm thành công');
                else
                    confirm('Thêm không thành công');
               
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }

        });
}
function open_chitiet(id,name) {
    $('#IDloai').val(id);
    $('#tenloai').val(name);
    $('#myMenuCH').modal('show');
    $('#themnemu').hide();
    $('#updatenemu').show();
    $('#deletemenu').show();
}
function setform(sp) {
    var form = document.getElementById('frmsp');
    form.action = '/CuaHang/UpdateSp';
    form.method = 'post';
    form.enctype = 'multipart/form-data';
    console.log(form);
    const ima = document.getElementById('hinh');
    $('#idsp').val(sp.productId);
    $('#idloai').val(sp.productCategory);
    $('#tensp').val(sp.productTitle);
    $('#Gia').val(sp.originalPrice);
    $('#productDesciptions').val(sp.productDesciptions);
    $('#productQuanlity').val(sp.productQuanlity);
    ima.src = sp.productIcon;
    $('#addSanp').modal('show');
    $('#themsp').hide();
    $('#updatesp').show();
    $('#deletesp').show();
}
function open_updatesp(mID) {
    $.ajax(
        {
            url: "/CuaHang/Layma/" + mID,
            type: "GET",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                setform(result);
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }

        });
}
function open_addsp(idloai) { 
    var form = document.getElementById('frmsp');
    form.action = '/CuaHang/ThemSpAsync';
    form.method = 'post';
    form.enctype = 'multipart/form-data';
    $('#idloai').val(idloai);

}
function Update() {
    var emp = {
        idLoai: $('#IDloai').val(),
        mName: $('#tenloai').val()
    };
    $.ajax(
        {
            url: "/CuaHang/Updatemenu",
            data: JSON.stringify(emp),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result==1)
                    alert('Cập nhật thành công');
                else
                    alert('Cập nhật không thành công');
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }

        });
}
function Delete() {
    var emp = {
        idLoai: $('#IDloai').val(),
        mName: $('#tenloai').val()
    };
    $.ajax(
        {
            url: "/CuaHang/Deletemenu",
            data: JSON.stringify(emp),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result==1)
                    alert('Xóa thành công');
                else
                    alert('Xóa không thành công');
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }

        });
}
const loadfile = document.getElementById('Hinhanh');
const ima = document.getElementById('hinh');
loadfile.addEventListener('change', function (e) {
    const file = e.target.files[0];
    ima.src = URL.createObjectURL(file);
    ima.alt = file.name;
})
function Deletesp() {
    var form = document.getElementById('frmsp');
    form.action = '';
    form.method = '';
    form.enctype = '';
    var id = $('#idsp').val();
    $.ajax(
        {
            url: "/CuaHang/Deletesp/"+id,
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result == 1)
                    alert('Xóa thành công');
                else
                    alert('Xóa không thành công');
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }

        });
}

//function xoahinh(url) {
//    var chuoi = $('#xoahinh').val();
//    chuoi += url;
//    $('#xoahinh').val(chuoi);
//    var img = Image
//}