using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webgrabfood.Models
{
    public class HoaDon
    {
        public string idHD { set; get; }
        public string idCh { set; get; }
        public string idKH { set; get; }
        public DateTime NgayDat { set; get; }
        public DateTime NgayGiao { set; get; }
        public int TongTien { set; get; }
        public string TinhTrang { set; get; }
    }
}