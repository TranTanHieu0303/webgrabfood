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
            AuthSecret = "WLiuHSmqhvphkmigEwyqYkQpCyHh0SmbQ3EA0eER",
            BasePath = "https://grabfood-7b5a8-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;
        //phuong thuc khoi tao
        public itemGioHang(string ms)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("SanPham/" + ms);
            SanPham sp = JsonConvert.DeserializeObject<SanPham>(response.Body);
            if (sp != null)
            {
                sMaCH = sp.idCH;
                sMaSanPham = ms;
                sTenSP = sp.nameDrink;
                sHinhAnh = sp.hinh;
                dDonGia = (int)sp.price;
                iSoLuong = 1;
            }
        }

    }
}