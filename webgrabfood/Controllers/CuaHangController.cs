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
            AuthSecret = "3EPUNova45ftx07snTnnjnWLiXtFKH2CtMXuoIWn",
            BasePath = "https://grapfood-7b658-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;

        // GET: CuaHang
        public ActionResult TrangChuCh()
        {

            CuaHang ch = Session["ch"] as CuaHang;
            string[] chuoihinh = ch.ImageURL.Split(',');
            ViewBag.chuoihinh = chuoihinh;
            return View(ch);
        }
        public ActionResult MenuCh()
        {
            CuaHang ch = Session["ch"] as CuaHang;
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Users/"+ch.UID+"/DanhMuc");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            List<Menu_CH> lstmenu = new List<Menu_CH>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    Menu_CH menu = JsonConvert.DeserializeObject<Menu_CH>(((JProperty)item).Value.ToString());
                    menu.idLoai = ((JProperty)item).Name.ToString();
                    lstmenu.Add(menu);

                }
            }
            return PartialView(lstmenu);
        }
        public ActionResult dssp()
        {
            CuaHang ch = Session["ch"] as CuaHang;
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Users/" + ch.UID + "/DanhMuc");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            List<Menu_CH> menu = new List<Menu_CH>();
            if (data != null)
                foreach (var item in data)
                {
                    Menu_CH mn = JsonConvert.DeserializeObject<Menu_CH>(((JProperty)item).Value.ToString());
                    mn.idLoai = ((JProperty)item).Name.ToString();
                    menu.Add(mn);
                }
            ViewData["Menu"] = menu;
            FirebaseResponse responsesp = client.Get("Users/" + ch.UID + "/Products");
            dynamic data1 = JsonConvert.DeserializeObject<dynamic>(responsesp.Body);
            List<SanPham> list = new List<SanPham>();
            if (data1 != null)
                foreach (var item in data1)
                {
                    SanPham sp = JsonConvert.DeserializeObject<SanPham>(((JProperty)item).Value.ToString());
                    //sp.productId = ((JProperty)item).Name.ToString();
                    list.Add(sp);
                }
            return PartialView(list);

        }
        
        [HttpGet]
        public ActionResult DangNhapCH()
        {
            CuaHang ch = null;
            Session["ch"] = ch;
            //client = new FireSharp.FirebaseClient(config);
            //FirebaseResponse response = client.Get("DanhMuc");
            List<DanhMuc> lst = new List<DanhMuc>();
            //dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            //foreach(var item in data)
            //{
            //    DanhMuc dm = JsonConvert.DeserializeObject<DanhMuc>(((JProperty)item).Value.ToString());
            //    dm.IDDanhmuc = ((JProperty)item).Name.ToString();
            //    lst.Add(dm);
            //}
            DanhMuc dm = new DanhMuc();
            dm.IDDanhmuc = "1";
            dm.mImage = "";
            dm.mName = "Tất Cả Quán Ăn";
            DanhMuc dm2 = new DanhMuc();
            dm2.IDDanhmuc = "2";
            dm2.mImage = "";
            dm2.mName = "Quán Ăn";
            DanhMuc dm3 = new DanhMuc();
            dm3.IDDanhmuc = "3";
            dm3.mImage = "";
            dm3.mName = "Coffe";
            DanhMuc dm4 = new DanhMuc();
            dm4.IDDanhmuc = "4";
            dm4.mImage = "";
            dm4.mName = "Trà sữa";
            lst.Add(dm);
            lst.Add(dm2);
            lst.Add(dm3);
            lst.Add(dm4);
            return View(lst);
        }

        [HttpPost]
        public ActionResult DangNhapCh( FormCollection f)
        {
            string usename = f["usename"];
            string pas = f["pass"];
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Users");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            foreach (var item in data)
            {
                Uesr user = JsonConvert.DeserializeObject<Uesr>(((JProperty)item).Value.ToString());
                if (user.AccountType == "Chef")
                {
                    CuaHang ch = JsonConvert.DeserializeObject<CuaHang>(((JProperty)item).Value.ToString());
                    if ((ch.EmailId == usename || ch.MobileNo == usename) && ch.Password == pas)
                    {
                       // ch.UID = ((JProperty)item).Name.ToString();
                        Session["ch"] = ch;
                        return RedirectToAction("TrangChuCh", "CuaHang");
                    }
                }
            }
            //FirebaseResponse response2 = client.Get("DanhMuc");
            List<DanhMuc> lst = new List<DanhMuc>();
            //dynamic data2 = JsonConvert.DeserializeObject<dynamic>(response.Body);
            //foreach (var item in data2)
            //{
            //    DanhMuc dm = JsonConvert.DeserializeObject<DanhMuc>(((JProperty)item).Value.ToString());
            //    dm.IDDanhmuc = ((JProperty)item).Name.ToString();
            //    lst.Add(dm);
            //}
            DanhMuc dm = new DanhMuc();
            dm.IDDanhmuc = "1";
            dm.mImage = "";
            dm.mName = "Tất Cả Quán Ăn";
            DanhMuc dm2 = new DanhMuc();
            dm2.IDDanhmuc = "2";
            dm2.mImage = "";
            dm2.mName = "Quán Ăn";
            DanhMuc dm3 = new DanhMuc();
            dm3.IDDanhmuc = "3";
            dm3.mImage = "";
            dm3.mName = "Coffe";
            DanhMuc dm4 = new DanhMuc();
            dm4.IDDanhmuc = "4";
            dm4.mImage = "";
            dm4.mName = "Trà sữa";
            lst.Add(dm);
            lst.Add(dm2);
            lst.Add(dm3);
            lst.Add(dm4);
            return View(lst);
        }
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> DangKyChAsync(FormCollection f,HttpPostedFileBase hinhanh)
        {
            string ten = hinhanh.FileName;
            Uesr ch = new Uesr();
            ch.AccountType = "Chef";
            ch.MobileNo = f["SDTCH"];
            ch.EmailId = f["gmailCH"];
            ch.NameShop = f["TenCH"];
            ch.City = f["provinceCH"].ToString();
            ch.State = f["districtCH"];
            ch.Area = f["wardCH"];
            ch.House = f["duongCH"];
            ch.Password = f["PassCH"];
            ch.CompleteAddress = ch.House + ", " + ch.Area + ", " + ch.State + ", " + ch.City;
            ch.LastName = "";
            ch.Latitude = "0.0";
            ch.Longitude = "0.0";
            ch.DeliveryFree = "";
            ch.Online = "false";
            ch.ShopOpen = "false";
            ch.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
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
            ch.ImageURL = downloadUrl;
            client = new FireSharp.FirebaseClient(config);
            PushResponse push = client.Push("Users", ch);
            ch.UID = push.Result.name;
            SetResponse set = client.Set("Users/"+push.Result.name, ch);
            if (set.StatusCode == System.Net.HttpStatusCode.OK)
                ViewBag.kq = true;
            else
                ViewBag.kq = false;
            Session["ch"] = ch;
            //FirebaseResponse response = client.Get("DanhMuc");
            List<DanhMuc> lst = new List<DanhMuc>();
            //dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            //foreach (var item in data)
            //{
            //    DanhMuc dm = JsonConvert.DeserializeObject<DanhMuc>(((JProperty)item).Value.ToString());
            //    dm.IDDanhmuc = ((JProperty)item).Name.ToString();
            //    lst.Add(dm);
            //}
            DanhMuc dm = new DanhMuc();
            dm.IDDanhmuc = "1";
            dm.mImage = "";
            dm.mName = "Tất Cả Quán Ăn";
            DanhMuc dm2 = new DanhMuc();
            dm2.IDDanhmuc = "2";
            dm2.mImage = "";
            dm2.mName = "Quán Ăn";
            DanhMuc dm3 = new DanhMuc();
            dm3.IDDanhmuc = "3";
            dm3.mImage = "";
            dm3.mName = "Coffe";
            DanhMuc dm4 = new DanhMuc();
            dm4.IDDanhmuc = "4";
            dm4.mImage = "";
            dm4.mName = "Trà sữa";
            lst.Add(dm);
            lst.Add(dm2);
            lst.Add(dm3);
            lst.Add(dm4);
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
            ch.ImageURL +=","+ downloadUrl;
            client = new FireSharp.FirebaseClient(config);
            await client.UpdateAsync("CuaHang/"+ch.UID,ch);
            string[] chuoihinh = ch.ImageURL.Split(',');
            ViewBag.chuoihinh = chuoihinh;
            return View("TrangChuCh",ch);
        }
        [HttpPost]
        public JsonResult Themmenu(Menu_CH menu)
        {
            CuaHang ch = Session["ch"] as CuaHang;
            if(menu.mName==null)
                return Json(0, JsonRequestBehavior.AllowGet);
            client = new FireSharp.FirebaseClient(config);
            PushResponse push = client.Push("Users/" + ch.UID+"/DanhMuc", menu);
            if (push.StatusCode == System.Net.HttpStatusCode.OK)
                return Json(1, JsonRequestBehavior.AllowGet);
            else
                return Json(0, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Updatemenu(Menu_CH menu)
        {

            CuaHang ch = Session["ch"] as CuaHang;
            if (menu.mName == null)
                return Json(0, JsonRequestBehavior.AllowGet);
            client = new FireSharp.FirebaseClient(config);
            SetResponse push = client.Set("Users/" + ch.UID + "/DanhMuc/" + menu.idLoai+"/mName", menu.mName);
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
            FirebaseResponse push = client.Delete("Users/" + ch.UID + "/DanhMuc/" + menu.idLoai);
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
            sp.uid = ch.UID;
            sp.productId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            sp.timestamp = sp.productId;
            sp.discountAvailable = "fasle";
            sp.discountNote = "";
            sp.discountPrice = "0";
            sp.productDesciptions = f["productDesciptions"];
            sp.productQuanlity = f["productQuanlity"];
            sp.productCategory = f["idloai"];
            sp.productTitle = f["tensp"];
            sp.originalPrice = int.Parse(f["Gia"]);
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
            sp.productIcon = downloadUrl;
            SetResponse push = client.Set("Users/" + ch.UID + "/Products/"+sp.productId, sp);
            if (push.StatusCode == System.Net.HttpStatusCode.OK)
                ViewBag.tb = "Thêm Món Ăn Thành Công";
            else
                ViewBag.tb = "Thêm Món Ăn Không Thành Công";
            return RedirectToAction("TrangChuCh");
        }
        public JsonResult Layma(string ID)
        {
            CuaHang ch = Session["ch"] as CuaHang;
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Users/" + ch.UID + "/Products/" + ID);
            SanPham sp = JsonConvert.DeserializeObject<SanPham>(response.Body);
            sp.productId = ID;
            return Json(sp, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateSp(FormCollection f, HttpPostedFileBase Hinhanh)
        {
            CuaHang ch = Session["ch"] as CuaHang;
            string id = f["idsp"];
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse responsesp = client.Get("Users/" + ch.UID + "/Products/" + id);
            SanPham sp = JsonConvert.DeserializeObject<SanPham>(responsesp.Body);
            sp.uid = ch.UID;
            sp.productCategory = f["idloai"];
            sp.productTitle = f["tensp"];
            sp.originalPrice = int.Parse(f["Gia"]);
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
                sp.productIcon = downloadUrl;
            }
            SetResponse push = client.Set("Users/" + ch.UID + "/Products/" + id, sp);
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
                CuaHang ch = Session["ch"] as CuaHang;
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse push = client.Delete("Users/" + ch.UID + "/Products/" + id);
                if (push.StatusCode == System.Net.HttpStatusCode.OK)
                    return Json(1, JsonRequestBehavior.AllowGet);
                else
                    return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public async Task<ActionResult> updateCuahangAsync(FormCollection f,HttpPostedFileBase hinhanh)
        {
            CuaHang ch = Session["ch"] as CuaHang;
            ch.MobileNo = f["SDTCH"];
            ch.EmailId = f["gmailCH"];
            ch.NameShop = f["TenCH"];
            string tp = f["provinceCH"];
            string quan = f["districtCH"];
            string phuong = f["wardCH"];
            string duong = f["duongCH"];
            if (tp != "" && tp != ch.City&& tp != "Tỉnh/Thành Phố")
                ch.City = tp;
            if (quan != "" && quan != ch.State)
                ch.City = tp;
            if (phuong != "" && phuong != ch.Area)
                ch.City = tp;
            if (duong != "" && duong != ch.House)
                ch.City = tp;
            ch.CompleteAddress = ch.House + ", " + ch.Area + ", " + ch.State + ", " + ch.City;
            if (hinhanh != null)
            {
                string ten = hinhanh.FileName;
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
                    .Child(DateTime.Now.Millisecond + ten)
                    .PutAsync(stream);

                // Track progress of the upload
                task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

                // await the task to wait until upload completes and get the download url
                var downloadUrl = await task;
                ch.ImageURL = downloadUrl;
            }
            client = new FireSharp.FirebaseClient(config);
            SetResponse push = client.Set("Users/" + ch.UID, ch);
            if (push.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ViewBag.tb = "Cập Nhật Món Ăn Thành Công";
                Session["ch"] = ch;
            }
            else
                ViewBag.tb = "Cập Nhật Món Ăn Không Thành Công";
            return RedirectToAction("TrangChuCh");
        }

    }
}