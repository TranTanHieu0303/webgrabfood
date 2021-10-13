using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webgrabfood.Models
{
    public class HoaDon
    {
        public string orderId { set; get; }
        public string orderTo { set; get; }
        public string orderBy { set; get; }
        //public DateTime NgayDat { set; get; }
        //public DateTime NgayGiao { set; get; }
        public int orderCost { set; get; }
        public string orderStatus { set; get; }
        public string orderTime { set; get; }
        public string latitude { set; get; }
        public string longitude { set; get; }
    }
}