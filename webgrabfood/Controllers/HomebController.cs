using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using webgrabfood.Models;

namespace webgrabfood.Controllers
{
    public class HomebController : ApiController
    {
        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "3EPUNova45ftx07snTnnjnWLiXtFKH2CtMXuoIWn",
            BasePath = "https://grapfood-7b658-default-rtdb.firebaseio.com/"
        };
        DataClasses1DataContext data = new DataClasses1DataContext(@"Data Source=DESKTOP-84OEQ03\SQLEXPRESS;Initial Catalog=QL_ThiTracNghiem;Integrated Security=True");
        IFirebaseClient client;
        [HttpGet]
        [Route("api/Homeb/Load")]
        public HttpResponseMessage Load()
        {
            List<TaiKhoan> taiKhoans = data.TaiKhoans.ToList();
            return Request.CreateResponse(HttpStatusCode.OK,taiKhoans);
        }
        [HttpGet]
        [Route("api/Homeb/Load_CuaHang")]
        public HttpResponseMessage Load_CuaHang()
        {
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
                    list.Add(ch);
                }

            };
            return Request.CreateResponse(HttpStatusCode.OK, list);
        }
        [HttpGet]
        [Route("api/Homeb/Select_MonAn/{MaCH}")]
        public HttpResponseMessage Select_MonAn(string MaCH)
        {
            client = new FireSharp.FirebaseClient(config);
            List<SanPham> lstsp = new List<SanPham>();
            FirebaseResponse response2 = client.Get("SanPham");
            dynamic datasp = JsonConvert.DeserializeObject<dynamic>(response2.Body);
            foreach (var item in datasp)
            {
                SanPham sp = JsonConvert.DeserializeObject<SanPham>(((JProperty)item).Value.ToString());
                if (sp.uid == MaCH)
                    sp.productId = ((JProperty)item).Name.ToString();
                lstsp.Add(sp);

            }
            if (lstsp.Count != 0)
                return Request.CreateResponse(HttpStatusCode.OK, lstsp);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound, new object { });
        }
        [HttpGet]
        [Route("api/Homeb/Them_gioHang/{Ma}")]
        public HttpResponseMessage Them_gioHang(string Ma)
        {
            GioHang gh = new GioHang();
            gh.Them(Ma,"ma");
            if (gh.list.Count != 0)
                return Request.CreateResponse(HttpStatusCode.OK, gh.list);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound, new object { });
        }
        [HttpPost]
        [Route("api/Homeb/LuuDonHang/{makh}/{mach}")]
        public HttpResponseMessage LuuDonHang([FromBody]GioHang gioHang, string makh,string mach)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
                HoaDon hd = new HoaDon();
                //hd.NgayDat = DateTime.Now;
                //hd.NgayGiao = DateTime.Now.AddMinutes(20);
                hd.orderCost = gioHang.TongThanhTien();
                hd.orderBy = makh;
                PushResponse push = client.Push("HoaDon/" + mach, hd);
                foreach (itemGioHang item in gioHang.list)
                {
                    ChitietHD ct = new ChitietHD();
                    ct.quantity = item.iSoLuong;
                    ct.price = item.dDonGia;
                    ct.cost = item.ThanhTien;
                    SetResponse set = client.Set("ChiTietHD/" + push.Result.name.ToString() + "/" + item.sMaSanPham, ct);
                }
                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, false);
            }
        }
    }

    }

