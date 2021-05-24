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
});
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
                confirm('Thêm thành công');
                $('#iclose').trigger('click');
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
    $('#idsp').val(sp.id);
    $('#idloai').val(sp.idCate);
    $('#tensp').val(sp.nameDrink);
    $('#Gia').val(sp.price);
    ima.src = sp.hinh;
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
                alert('Cập nhật thành công');
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
                alert('Xóa thành công');
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