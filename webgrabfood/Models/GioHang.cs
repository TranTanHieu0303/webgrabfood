using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webgrabfood.Models
{
    public class GioHang
    {
        public List<itemGioHang> list;
        public GioHang()
        {
            list = new List<itemGioHang>();
        }
        public GioHang(List<itemGioHang> ds)
        {
            list = ds;
        }

        public int Somathang()
        {
            if (list == null)
                return 0;
            return list.Count();
        }
        public int TongSL()
        {
            if (list == null)
                return 0;
            return list.Sum(t => t.iSoLuong);
        }
        public int TongThanhTien()
        {
            if (list == null)
                return 0;
            return list.Sum(t => t.ThanhTien);
        }
        //pt xu ly
        public int Them(string ms)
        {
            itemGioHang sp = list.SingleOrDefault(g => g.sMaSanPham == ms);
            if (sp == null)
            {
                itemGioHang a = new itemGioHang(ms);
                if (a == null)
                    return 0;
                if (list.Count != 0)
                {
                    if (a.sMaCH != list[0].sMaCH)
                        list = new List<itemGioHang>();
                }
                list.Add(a);
            }
            else
            {
                sp.iSoLuong += 1;
            }
            return 1;
        }
        public int Bo(string ms)
        {
            itemGioHang sp = list.SingleOrDefault(g => g.sMaSanPham == ms);
            sp.iSoLuong -= 1;
            if (sp.iSoLuong == 0)
                list.Remove(sp);
            return 1;
        }
    }
}