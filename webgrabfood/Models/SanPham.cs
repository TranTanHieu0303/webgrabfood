using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webgrabfood.Models
{
    public class SanPham
    {
        public string productIcon { get; set; }
        public string productId { get; set; }
        public string productTitle { get; set; }
        public int originalPrice { get; set; }
        public string productCategory { get; set; }
        public string uid { get; set; }
        public string discountAvailable { get; set; }
        public string discountNote { get; set; }
        public string discountPrice { get; set; }
        public string productDesciptions { get; set; }
        public string productQuanlity { get; set; }
        public string timestamp { get; set; }
    }
}