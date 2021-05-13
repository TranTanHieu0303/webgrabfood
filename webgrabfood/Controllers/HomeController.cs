using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Firebase.Database;
using Firebase.Storage;
using Firebase.Database.Query;
using webgrabfood.Models;
using FireSharp.Interfaces;
using FireSharp.Config;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace webgrabfood.Controllers
{
    public class HomeController : Controller
    {
        
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "GrVis4EhUlfs5UWUztLMbDGBup7NirfZgHcazmTv",
            BasePath = "https://webgrabfood-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;

        public ActionResult Index()
        {
            
            var storage = new FirebaseStorage("gs://webgrabfood.appspot.com/");
            storage.Child("coca.jpg");
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Drinks");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            List<SanPham> list = new List<SanPham>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<SanPham>(((JProperty)item).Value.ToString()));
            };
            return View(list);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}