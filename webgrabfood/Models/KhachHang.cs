using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webgrabfood.Models
{
    public class KhachHang
    {
        public string UID { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string State { get; set; }
        public string House { get; set; }
        public string Password { get; set; }
        public string ImageURL { get; set; }
        public string CompleteAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}