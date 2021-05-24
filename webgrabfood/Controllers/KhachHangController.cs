using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webgrabfood.Models;
using FireSharp.Interfaces;
using FireSharp.Config;
using Firebase.Auth;
using FireSharp.Response;
using Firebase.Database;
using Firebase.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace webgrabfood.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: KhachHang
        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "WLiuHSmqhvphkmigEwyqYkQpCyHh0SmbQ3EA0eER",
            BasePath = "https://grabfood-7b5a8-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;
        [HttpGet]
        public ActionResult DangNhap()
        {
            KhachHang kh = null;
            Session["kh"] = kh;
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            string usename = f["usename"];
            string pas = f["pass"];
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("KhachHang");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            foreach(var item in data)
            {
                KhachHang kh = JsonConvert.DeserializeObject<KhachHang>(((JProperty)item).Value.ToString());
                if ((kh.Gmail==usename|| kh.SDT == usename)&&kh.Password==pas)
                {
                    kh.idKH= ((JProperty)item).Name.ToString();
                    Session["kh"] = kh;
                    CuaHang ch = Session["ctch"] as CuaHang;
                    if (ch == null)
                        return RedirectToAction("TrangChu", "Home");
                    else
                        return RedirectToAction("ChiTiet", "Home", new { ma = ch.idCH });

                }
            }
            return View();
        }


        [HttpPost]
        public JsonResult  DangKy(KhachHang k)
        {

            client = new FireSharp.FirebaseClient(config);

                PushResponse set = client.Push("KhachHang",k);
                if (set.StatusCode == System.Net.HttpStatusCode.OK)
                {
                return Json(1, JsonRequestBehavior.AllowGet);
                }
                else
                return Json(0, JsonRequestBehavior.AllowGet);



        }
        [HttpGet]
        public ActionResult test()
        {
            return View();
        }
        [HttpPost]
        public ActionResult test(FormCollection fi)
        {

            //var stream = fi.InputStream;

            ////authentication
            //var auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig("AIzaSyCl42UQ0cpTZqV5PJ_djoIQT6zuoPkm6WE"));
            //var a = await auth.SignInWithEmailAndPasswordAsync("trantanhieu1804@gmail.com", "Nguyenthihau1403");

            //// Constructr FirebaseStorage, path to where you want to upload the file and Put it there
            //var task = new FirebaseStorage(
            //    "webgrabfood.appspot.com",


            //     new FirebaseStorageOptions
            //     {
            //         AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
            //         ThrowOnCancel = true,
            //     })
            //    .Child("img"+DateTime.Now.Millisecond+".png")
            //    .PutAsync(stream);

            //// Track progress of the upload
            //task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

            //// await the task to wait until upload completes and get the download url
            //var downloadUrl = await task;
             return View();
        }
    }
}