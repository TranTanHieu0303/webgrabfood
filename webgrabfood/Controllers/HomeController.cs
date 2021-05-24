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

namespace webgrabfood.Controllers
{
    public class HomeController : Controller
    {

        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "WLiuHSmqhvphkmigEwyqYkQpCyHh0SmbQ3EA0eER",
            BasePath = "https://grabfood-7b5a8-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;
        public ActionResult TrangChu()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("CuaHang");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            List<CuaHang> list = new List<CuaHang>();
            foreach (var item in data)
            {
                CuaHang ch = JsonConvert.DeserializeObject<CuaHang>(((JProperty)item).Value.ToString());
                ch.idCH = ((JProperty)item).Name.ToString();
                list.Add(ch);
            };
            ViewBag.td = "Tất Cả Quán Ăn";
            return View(list);
        }
        public ActionResult taikhoan()
        {
            KhachHang kh = Session["kh"] as KhachHang;
            return PartialView(kh);
        }
        public ActionResult DanhMuc()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("DanhMuc");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            List<DanhMuc> list = new List<DanhMuc>();
            foreach (var item in data)
            {
                DanhMuc sp = JsonConvert.DeserializeObject<DanhMuc>(((JProperty)item).Value.ToString());
                sp.IDDanhmuc = ((JProperty)item).Name.ToString();
                list.Add(sp);
            };
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
                ch.idCH = ((JProperty)item).Name.ToString();
                if (ch.idDM == loai.IDDanhmuc)
                    list.Add(ch);
            };
            ViewBag.td = loai.mName;
            return View("TrangChu", list);
        }
        public ActionResult ChiTiet(string ma)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("CuaHang/" + ma);
            CuaHang ch = JsonConvert.DeserializeObject<CuaHang>(response.Body);
            ch.idCH = ma;
            Session["ctch"] = ch;
            FirebaseResponse response1 = client.Get("Menu_CH/" + ch.idCH);
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response1.Body);
            List<Menu_CH> lstmenu = new List<Menu_CH>();
            foreach (var item in data)
            {
                Menu_CH menu = JsonConvert.DeserializeObject<Menu_CH>(((JProperty)item).Value.ToString());
                menu.idLoai = ((JProperty)item).Name.ToString();
                lstmenu.Add(menu);

            }
            FirebaseResponse response2 = client.Get("SanPham");
            dynamic datasp = JsonConvert.DeserializeObject<dynamic>(response2.Body);
            List<SanPham> lstsp = new List<SanPham>();
            foreach (var item in datasp)
            {
                SanPham sp = JsonConvert.DeserializeObject<SanPham>(((JProperty)item).Value.ToString());
                if(sp.idCH == ch.idCH)
                sp.id = ((JProperty)item).Name.ToString();
                lstsp.Add(sp);

            }
            
            string[] chuoihinh = ch.HinhAnh.Split(',');
            ViewBag.chuoihinh = chuoihinh;
            ViewData["Menu"] = lstmenu;
            ViewData["SanPham"] = lstsp;
            return View(ch);
        }
        [HttpGet]
        public ActionResult ThemDanhMuc()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ThemDanhMuc(FormCollection f , HttpPostedFileBase Hinhanh)
        {
            DanhMuc md = new DanhMuc();
            md.mName = f["tenmuc"];
            if(Hinhanh!=null)
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
        public ActionResult themgiohang(string ma )
        {
            KhachHang kh = Session["kh"] as KhachHang;
            if (kh == null)
                return RedirectToAction("DangNhap", "KhachHang");
            GioHang gh = (GioHang)Session["gh"];
            if (gh == null)
            {
                gh = new GioHang();
            }
            gh.Them(ma);
            Session["gh"] = gh;
            CuaHang ch = Session["ctch"] as CuaHang;
            return RedirectToAction("ChiTiet",new { ma = ch.idCH});
        }
        public ActionResult giohang()
        {
            GioHang gio = (GioHang)Session["gh"];
            return PartialView(gio);
        }
        [HttpPost]
        public ActionResult LuuDonHang(FormCollection f)
        {
            client = new FireSharp.FirebaseClient(config);
            KhachHang khach = Session["kh"] as KhachHang;
            GioHang gio = Session["gh"] as GioHang;
            CuaHang ch = Session["ctch"] as CuaHang;
            try  
            {
                string ngaygiao = f["txtDate"];
                HoaDon hd = new HoaDon();
                hd.NgayDat = DateTime.Now;
                hd.NgayGiao = DateTime.Parse(ngaygiao);
                hd.TongTien = gio.TongThanhTien();
                hd.idKH = khach.idKH;
                PushResponse push = client.Push("HoaDon/"+ch.idCH, hd);
                foreach (itemGioHang item in gio.list)
                {
                    ChitietHD ct = new ChitietHD();
                    ct.SoLuong = item.iSoLuong;
                    ct.GiaBan = item.dDonGia;
                    ct.TongTien = item.ThanhTien;
                    SetResponse set = client.Set("ChiTietHD/" + push.Result.name.ToString() + "/" + item.sMaSanPham, ct);
                }
                Session["gh"] = null;
                ViewBag.tb = true;
                return RedirectToAction("ChiTiet","Home", new { ma = ch.idCH });
            }
            catch
            {
                ViewBag.tb = false;
                return RedirectToAction("ChiTiet", "Home", new { ma = ch.idCH });
            }
        }
    }
}