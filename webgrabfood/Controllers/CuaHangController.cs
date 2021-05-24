using Firebase.Auth;
using Firebase.Storage;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using webgrabfood.Models;
namespace webgrabfood.Controllers
{
    public class CuaHangController : Controller
    {
        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "WLiuHSmqhvphkmigEwyqYkQpCyHh0SmbQ3EA0eER",
            BasePath = "https://grabfood-7b5a8-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;

        // GET: CuaHang
        public ActionResult TrangChuCh()
        {
            CuaHang ch = Session["ch"] as CuaHang;
            string[] chuoihinh = ch.HinhAnh.Split(',');
            ViewBag.chuoihinh = chuoihinh;
            return View(ch);
        }
        public ActionResult MenuCh()
        {
            CuaHang ch = Session["ch"] as CuaHang;
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Menu_CH/"+ch.idCH);
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            List<Menu_CH> lstmenu = new List<Menu_CH>();
            foreach(var item in data)
            {
                Menu_CH menu = JsonConvert.DeserializeObject<Menu_CH>(((JProperty)item).Value.ToString());
                menu.idLoai = ((JProperty)item).Name.ToString();
                lstmenu.Add(menu);

            }
            return PartialView(lstmenu);
        }
        public ActionResult dssp()
        {
            CuaHang ch = Session["ch"] as CuaHang;
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Menu_CH/" + ch.idCH);
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            List<Menu_CH> menu = new List<Menu_CH>();
            foreach (var item in data)
            {
                Menu_CH mn = JsonConvert.DeserializeObject<Menu_CH>(((JProperty)item).Value.ToString());
                mn.idLoai = ((JProperty)item).Name.ToString();
                menu.Add(mn);
            }
            ViewData["Menu"] = menu;
            FirebaseResponse responsesp = client.Get("SanPham");
            dynamic data1 = JsonConvert.DeserializeObject<dynamic>(responsesp.Body);
            List<SanPham> list = new List<SanPham>();
            foreach (var item in data1)
            {
                SanPham sp = JsonConvert.DeserializeObject<SanPham>(((JProperty)item).Value.ToString());
                sp.id = ((JProperty)item).Name.ToString();
                list.Add(sp);
            };
            return PartialView(list);

        }
        
        [HttpGet]
        public ActionResult DangNhapCH()
        {
            CuaHang ch = null;
            Session["ch"] = ch;
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("DanhMuc");
            List<DanhMuc> lst = new List<DanhMuc>();
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            foreach(var item in data)
            {
                DanhMuc dm = JsonConvert.DeserializeObject<DanhMuc>(((JProperty)item).Value.ToString());
                dm.IDDanhmuc = ((JProperty)item).Name.ToString();
                lst.Add(dm);
            }    
            return View(lst);
        }

        [HttpPost]
        public ActionResult DangNhapCh( FormCollection f)
        {
            string usename = f["usename"];
            string pas = f["pass"];
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("CuaHang");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            foreach (var item in data)
            {
                CuaHang ch = JsonConvert.DeserializeObject<CuaHang>(((JProperty)item).Value.ToString());
                if ((ch.Gmail == usename || ch.SDT == usename) && ch.Password == pas)
                {
                    ch.idCH = ((JProperty)item).Name.ToString();
                    Session["ch"] = ch;
                    return RedirectToAction("TrangChuCh", "CuaHang");
                }
            }
            return View();
        }
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> DangKyChAsync(FormCollection f,HttpPostedFileBase hinhanh)
        {
            string ten = hinhanh.FileName;
            CuaHang ch = new CuaHang();
            ch.SDT = f["SDTCH"];
            ch.Gmail = f["gmailCH"];
            ch.TenCH = f["TenCH"];
            ch.MoTa = f["Mota"];
            ch.idDM = f["Loaiquan"];
            ch.TinhThanh = f["provinceCH"].ToString();
            ch.QuanHuyen = f["districtCH"];
            ch.PhuongXa = f["wardCH"];
            ch.SonhaTenDuong = f["duongCH"];
            ch.Password = f["PassCH"];
            var stream = hinhanh.InputStream;

            //authentication
            var auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig("AIzaSyBeRJmjrBFEXy1u1BeyNDmCByq03INNaJs"));
            var a = await auth.SignInWithEmailAndPasswordAsync("trantanhieu1804@gmail.com", "Nguyenthihau1403");

            // Constructr FirebaseStorage, path to where you want to upload the file and Put it there
            var task = new FirebaseStorage(
                "grabfood-7b5a8.appspot.com",


                 new FirebaseStorageOptions
                 {
                     AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                     ThrowOnCancel = true,
                 })
                .Child("Image")
                .Child(DateTime.Now.Millisecond+ten)
                .PutAsync(stream);

            // Track progress of the upload
            task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

            // await the task to wait until upload completes and get the download url
            var downloadUrl = await task;
            ch.HinhAnh = downloadUrl;
            client = new FireSharp.FirebaseClient(config);
            PushResponse set = client.Push("CuaHang",ch);
            if (set.StatusCode == System.Net.HttpStatusCode.OK)
                ViewBag.kq = true;
            else
                ViewBag.kq = false;
            Session["ch"] = ch;
            FirebaseResponse response = client.Get("DanhMuc");
            List<DanhMuc> lst = new List<DanhMuc>();
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            foreach (var item in data)
            {
                DanhMuc dm = JsonConvert.DeserializeObject<DanhMuc>(((JProperty)item).Value.ToString());
                dm.IDDanhmuc = ((JProperty)item).Name.ToString();
                lst.Add(dm);
            }
            return View("DangNhapCH",lst);
        }
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> themhinhAsync( HttpPostedFileBase hinh)
        {
            CuaHang ch = Session["ch"] as CuaHang;
            string ten = hinh.FileName;
            var stream = hinh.InputStream;

            //authentication
            var auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig("AIzaSyBeRJmjrBFEXy1u1BeyNDmCByq03INNaJs"));
            var a = await auth.SignInWithEmailAndPasswordAsync("trantanhieu1804@gmail.com", "Nguyenthihau1403");

            // Constructr FirebaseStorage, path to where you want to upload the file and Put it there
            var task = new FirebaseStorage(
                "grabfood-7b5a8.appspot.com",


                 new FirebaseStorageOptions
                 {
                     AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                     ThrowOnCancel = true,
                 })
                .Child("Image")
                .Child(DateTime.Now.Millisecond + ten)
                .PutAsync(stream);

            // Track progress of the upload
            task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

            // await the task to wait until upload completes and get the download url
            var downloadUrl = await task;
            ch.HinhAnh +=","+ downloadUrl;
            client = new FireSharp.FirebaseClient(config);
            await client.UpdateAsync("CuaHang/"+ch.idCH,ch);
            string[] chuoihinh = ch.HinhAnh.Split(',');
            ViewBag.chuoihinh = chuoihinh;
            return View("TrangChuCh",ch);
        }
        [HttpPost]
        public JsonResult Themmenu(Menu_CH menu)
        {
            CuaHang ch = Session["ch"] as CuaHang;


            client = new FireSharp.FirebaseClient(config);
            PushResponse push = client.Push("Menu_CH/" + ch.idCH, menu);
            if (push.StatusCode == System.Net.HttpStatusCode.OK)
                return Json(1, JsonRequestBehavior.AllowGet);
            else
                return Json(0, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Updatemenu(Menu_CH menu)
        {

            CuaHang ch = Session["ch"] as CuaHang;
            client = new FireSharp.FirebaseClient(config);
            SetResponse push = client.Set("Menu_CH/" + ch.idCH + "/" + menu.idLoai+"/mName", menu.mName);
            if (push.StatusCode == System.Net.HttpStatusCode.OK)
                return Json(1, JsonRequestBehavior.AllowGet);
            else
                return Json(0, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Deletemenu(Menu_CH menu)
        {

            CuaHang ch = Session["ch"] as CuaHang;
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse push = client.Delete("Menu_CH/" + ch.idCH + "/" + menu.idLoai);
            if (push.StatusCode == System.Net.HttpStatusCode.OK)
                return Json(1, JsonRequestBehavior.AllowGet);
            else
                return Json(0, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> ThemSpAsync(FormCollection f , HttpPostedFileBase Hinhanh)
        {
            CuaHang ch = Session["ch"] as CuaHang;
            client = new FireSharp.FirebaseClient(config);
            SanPham sp = new SanPham();
            sp.idCH = ch.idCH;
            sp.idCate = f["idloai"];
            sp.nameDrink = f["tensp"];
            sp.price = int.Parse(f["Gia"]);
            string ten = Hinhanh.FileName;
            var stream = Hinhanh.InputStream;

            //authentication
            var auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig("AIzaSyBeRJmjrBFEXy1u1BeyNDmCByq03INNaJs"));
            var a = await auth.SignInWithEmailAndPasswordAsync("trantanhieu1804@gmail.com", "Nguyenthihau1403");

            // Constructr FirebaseStorage, path to where you want to upload the file and Put it there
            var task = new FirebaseStorage(
                "grabfood-7b5a8.appspot.com",


                 new FirebaseStorageOptions
                 {
                     AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                     ThrowOnCancel = true,
                 })
                .Child("Image")
                .Child(DateTime.Now.Millisecond + ten)
                .PutAsync(stream);

            // Track progress of the upload
            task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

            // await the task to wait until upload completes and get the download url
            var downloadUrl = await task;
            sp.hinh = downloadUrl;
            PushResponse push = client.Push("SanPham",sp);
            if (push.StatusCode == System.Net.HttpStatusCode.OK)
                ViewBag.tb = "Thêm Món Ăn Thành Công";
            else
                ViewBag.tb = "Thêm Món Ăn Không Thành Công";
            return RedirectToAction("TrangChuCh");
        }
        public JsonResult Layma(string ID)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("SanPham/" + ID);
            SanPham sp = JsonConvert.DeserializeObject<SanPham>(response.Body);
            sp.id = ID;
            return Json(sp, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateSp(FormCollection f, HttpPostedFileBase Hinhanh)
        {
            CuaHang ch = Session["ch"] as CuaHang;
            string id = f["idsp"];
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse responsesp = client.Get("SanPham/"+id);
            SanPham sp = JsonConvert.DeserializeObject<SanPham>(responsesp.Body);
            sp.idCH = ch.idCH;
            sp.idCate = f["idloai"];
            sp.nameDrink = f["tensp"];
            sp.price = int.Parse(f["Gia"]);
            if (Hinhanh != null)
            {
                string ten = Hinhanh.FileName;
                var stream = Hinhanh.InputStream;

                //authentication
                var auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig("AIzaSyBeRJmjrBFEXy1u1BeyNDmCByq03INNaJs"));
                var a = await auth.SignInWithEmailAndPasswordAsync("trantanhieu1804@gmail.com", "Nguyenthihau1403");

                // Constructr FirebaseStorage, path to where you want to upload the file and Put it there
                var task = new FirebaseStorage(
                    "grabfood-7b5a8.appspot.com",


                     new FirebaseStorageOptions
                     {
                         AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                         ThrowOnCancel = true,
                     })
                    .Child("Image")
                    .Child(DateTime.Now.Millisecond + ten)
                    .PutAsync(stream);

                // Track progress of the upload
                task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

                // await the task to wait until upload completes and get the download url
                var downloadUrl = await task;
                sp.hinh = downloadUrl;
            }
            SetResponse push = client.Set("SanPham/"+id, sp);
            if (push.StatusCode == System.Net.HttpStatusCode.OK)
                ViewBag.tb = "Cập Nhật Món Ăn Thành Công";
            else
                ViewBag.tb = "Cập Nhật Món Ăn Không Thành Công";
            return RedirectToAction("TrangChuCh");
        }
        [HttpPost]
        public JsonResult Deletesp(string id)
        {
            if(id != "undefined")
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse push = client.Delete("SanPham/" + id);
                if (push.StatusCode == System.Net.HttpStatusCode.OK)
                    return Json(1, JsonRequestBehavior.AllowGet);
                else
                    return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);

        }
        
    }
}