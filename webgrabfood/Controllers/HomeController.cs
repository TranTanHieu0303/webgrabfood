using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Firebase.Database;
using Firebase.Storage;
using System.Threading.Tasks;
using Firebase.Database.Query;
using webgrabfood.Models;
using FireSharp.Interfaces;
using FireSharp.Config;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Firebase.Auth;
using System.Net.Http;
using System.Net;
using System.Web.Http;
namespace webgrabfood.Controllers
{
    public class HomeController : Controller
    {

        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "3EPUNova45ftx07snTnnjnWLiXtFKH2CtMXuoIWn",
            BasePath = "https://grapfood-7b658-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;
        List<Menu_CH> lstmenu = new List<Menu_CH>();
        List<SanPham> lstsp = new List<SanPham>();
        public ActionResult TrangChu()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Users");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            List<CuaHang> list = new List<CuaHang>();
            foreach (var item in data)
            {
                Uesr user = JsonConvert.DeserializeObject<Uesr>(((JProperty)item).Value.ToString());
                if(user.AccountType == "Chef")
                {
                    CuaHang ch = JsonConvert.DeserializeObject<CuaHang>(((JProperty)item).Value.ToString());
                    list.Add(ch);
                }    
                
            };
            ViewBag.td = "Tất Cả Quán Ăn";
            return View(list);
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult KetQuaTimKiem(FormCollection f)
        {
            string t = f["txtTK"];
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Users");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            List<CuaHang> list = new List<CuaHang>();
            foreach (var item in data)
            {
                Uesr user = JsonConvert.DeserializeObject<Uesr>(((JProperty)item).Value.ToString());
                if (user.AccountType == "Chef")
                {
                    CuaHang ch = JsonConvert.DeserializeObject<CuaHang>(((JProperty)item).Value.ToString());
                    // ch.idCH = ((JProperty)item).Name.ToString();
                    if (ch.NameShop!=null &&ch.NameShop.ToLower().Trim().Contains(t.ToLower().Trim()))
                        list.Add(ch);
                }
            };
            ViewBag.td = "Kết quả tìm kiếm của '" + t + "'";
            return View("TrangChu", list);
        }
        public ActionResult taikhoan()
        {
            KhachHang kh = Session["kh"] as KhachHang;
            return PartialView(kh);
        }
        public ActionResult DanhMuc()
        {
            //client = new FireSharp.FirebaseClient(config);
            //FirebaseResponse response = client.Get("DanhMuc");
            //dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            List<DanhMuc> list = new List<DanhMuc>();
            //foreach (var item in data)
            //{
            //    DanhMuc sp = JsonConvert.DeserializeObject<DanhMuc>(((JProperty)item).Value.ToString());
            //    sp.IDDanhmuc = ((JProperty)item).Name.ToString();
            //    list.Add(sp);
            //};
            DanhMuc dm = new DanhMuc();
            dm.IDDanhmuc = "1";
            dm.mImage = "https://firebasestorage.googleapis.com/v0/b/grbfood-7b3e4.appspot.com/o/t%E1%BA%A3i%20xu%E1%BB%91ng%20(1).jfif?alt=media&token=37c49259-3ac5-41aa-81af-344dbaaa51d3";
            dm.mName = "Tất Cả Quán Ăn";
            DanhMuc dm2 = new DanhMuc();
            dm2.IDDanhmuc = "2";
            dm2.mImage = "https://firebasestorage.googleapis.com/v0/b/grbfood-7b3e4.appspot.com/o/84.jpg?alt=media&token=399c1f62-5aa3-4aa9-b2bb-d2362df2f39e";
            dm2.mName = "Quán Ăn";
            DanhMuc dm3 = new DanhMuc();
            dm3.IDDanhmuc = "3";
            dm3.mImage = "https://firebasestorage.googleapis.com/v0/b/grbfood-7b3e4.appspot.com/o/hinh-anh-ly-cafe-buoi-sang-dep-hinhminhhoa-1592099048025598964586-crop-1592099057052994975563.jpg?alt=media&token=e2dc7b50-27e3-478d-9d5c-3277f666cd09";
            dm3.mName = "Coffe";
            DanhMuc dm4 = new DanhMuc();
            dm4.IDDanhmuc = "4";
            dm4.mImage = "https://firebasestorage.googleapis.com/v0/b/grbfood-7b3e4.appspot.com/o/tra-sua-tu-lam.jpg?alt=media&token=d4c78cc1-1234-410c-bef5-91d0f3bf6344";
            dm4.mName = "Trà sữa";
            list.Add(dm);
            list.Add(dm2);
            list.Add(dm3);
            list.Add(dm4);
            return PartialView(list);
        }
        public ActionResult HTTheoLoaiSP(string ml)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("DanhMuc/" + ml);
            DanhMuc loai = JsonConvert.DeserializeObject<DanhMuc>(response.Body);
            loai.IDDanhmuc = ml;
            FirebaseResponse responsesp = client.Get("CuaHang");
            dynamic data1 = JsonConvert.DeserializeObject<dynamic>(responsesp.Body);
            List<CuaHang> list = new List<CuaHang>();
            foreach (var item in data1)
            {
                CuaHang ch = JsonConvert.DeserializeObject<CuaHang>(((JProperty)item).Value.ToString());
               // ch.idCH = ((JProperty)item).Name.ToString();
                if (ch.idDM == loai.IDDanhmuc)
                    list.Add(ch);
            };
            ViewBag.td = loai.mName;
            return View("TrangChu", list);
        }
        public ActionResult ChiTiet(string ma)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Users/" + ma);
            CuaHang ch = JsonConvert.DeserializeObject<CuaHang>(response.Body);
            Session["ctch"] = ch;
            FirebaseResponse response1 = client.Get("Users/" + ch.UID+"/DanhMuc");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response1.Body);
            foreach (var item in data)
            {
                Menu_CH menu = JsonConvert.DeserializeObject<Menu_CH>(((JProperty)item).Value.ToString());
                menu.idLoai = ((JProperty)item).Name.ToString();
                lstmenu.Add(menu);

            }
            FirebaseResponse response2 = client.Get("Users/" + ch.UID+ "/Products");
            dynamic datasp = JsonConvert.DeserializeObject<dynamic>(response2.Body);
            foreach (var item in datasp)
            {
                SanPham sp = JsonConvert.DeserializeObject<SanPham>(((JProperty)item).Value.ToString());
                lstsp.Add(sp);

            }

            string[] chuoihinh = ch.ImageURL.Split(',');
            ViewBag.chuoihinh = chuoihinh;
            ViewData["Menu"] = lstmenu;
            ViewData["SanPham"] = lstsp;
            return View(ch);
        }
        [System.Web.Mvc.HttpGet]
        public ActionResult ThemDanhMuc()
        {
            return View();
        }
        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> ThemDanhMuc(FormCollection f, HttpPostedFileBase Hinhanh)
        {
            DanhMuc md = new DanhMuc();
            md.mName = f["tenmuc"];
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
                md.mImage = downloadUrl;
            }
            else
            {
                md.mImage = "";
            }
            client = new FireSharp.FirebaseClient(config);
            PushResponse push = client.Push("DanhMuc", md);
            if (push.StatusCode == System.Net.HttpStatusCode.OK)
                ViewBag.tb = "Them Thanh Cong";
            else
                ViewBag.tb = "Khong Thanh Cong";
            return View();
        }
        public ActionResult themgiohang(string ma)
        {
            KhachHang kh = Session["kh"] as KhachHang;
            if (kh == null)
                return RedirectToAction("DangNhap", "KhachHang");
            GioHang gh = (GioHang)Session["gh"];
            if (gh == null)
            {
                gh = new GioHang();
            }
            CuaHang ch = Session["ctch"] as CuaHang;
            gh.Them(ma,ch.UID);
            Session["gh"] = gh;
            return RedirectToAction("ChiTiet", new { ma = ch.UID });
        }
        public ActionResult giohang()
        {
            GioHang gio = (GioHang)Session["gh"];
            return PartialView(gio);
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult LuuDonHang(FormCollection f)
        {
            client = new FireSharp.FirebaseClient(config);
            KhachHang khach = Session["kh"] as KhachHang;
            GioHang gio = Session["gh"] as GioHang;
            CuaHang ch = Session["ctch"] as CuaHang;
            string tinh = f["provinceCH"];
            if (tinh != "Tỉnh/Thành Phố")
            {
                if (ch.City != tinh)
                {
                    ViewBag.tb = 2;
                    string[] chuoihinh = ch.ImageURL.Split(',');
                    ViewBag.chuoihinh = chuoihinh;
                    ViewData["Menu"] = lstmenu;
                    ViewData["SanPham"] = lstsp;
                    return View("ChiTiet", ch);
                }
            }
            try
            {
                string ngaygiao = f["txtDate"];
                HoaDon hd = new HoaDon();
                //hd.NgayDat = null;
                //hd.NgayGiao = null;
                hd.orderCost = gio.TongThanhTien();
                hd.orderBy = khach.UID;
                hd.orderTo = ch.UID;
                hd.latitude = "10.7995323";
                hd.longitude = "106.6212262";
                hd.orderId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
                hd.orderTime = hd.orderId;
                SetResponse push = client.Set("Users/" + ch.UID + "/Orders/"+hd.orderId, hd);
                foreach (itemGioHang item in gio.list)
                {
                    ChitietHD ct = new ChitietHD();
                    ct.quantity = item.iSoLuong;
                    ct.price = item.dDonGia;
                    ct.cost = item.ThanhTien;
                    ct.pId = item.sMaSanPham;
                    ct.name = item.sTenSP;
                    SetResponse set = client.Set("Users/" + ch.UID + "/Orders/" + hd.orderId+ "/Items/" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(), ct);
                }
                Session["gh"] = null;
                ViewBag.tb = 1;
                return RedirectToAction("ChiTiet", "Home", new { ma = ch.UID });
            }
            catch
            {
                ViewBag.tb = 0;
                return RedirectToAction("ChiTiet", "Home", new { ma = ch.UID });
            }
        }
        
    }
}