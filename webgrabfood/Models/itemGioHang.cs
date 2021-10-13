using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FireSharp.Interfaces;
using FireSharp.Config;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace webgrabfood.Models
{
    public class itemGioHang
    {
        public string sMaCH { get; set; }
        public string sMaSanPham { get; set; }
        public string sTenSP { get; set; }
        public string sHinhAnh { get; set; }
        public int dDonGia { get; set; }
        public int iSoLuong { get; set; }
        public int ThanhTien
        {
            get { return iSoLuong * dDonGia; }

        }
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "3EPUNova45ftx07snTnnjnWLiXtFKH2CtMXuoIWn",
            BasePath = "https://grapfood-7b658-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;
        //phuong thuc khoi tao
        public itemGioHang(string ms,string mch)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Users/" + mch + "/Products/" + ms);
            SanPham sp = JsonConvert.DeserializeObject<SanPham>(response.Body);
            if (sp != null)
            {
                sMaCH = sp.uid;
                sMaSanPham = ms;
                sTenSP = sp.productTitle;
                sHinhAnh = sp.productIcon;
                dDonGia = (int)sp.originalPrice;
                iSoLuong = 1;
            }
        }

    }
}