using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webgrabfood.Models
{
    public class SanPham
    {
        public string hinh { get; set; }
        public string id { get; set; }
        public string nameDrink { get; set; }
        public int price { get; set; }
        public string idCate { get; set; }
        public string idCH { get; set; }
    }
}