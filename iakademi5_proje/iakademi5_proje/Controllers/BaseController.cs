using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iakademi5_proje.Models;

namespace iakademi5_proje.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        // SPECİAL PRODUCTS > günün ürünü -> tbl_products(status kolunu = 1)
        // Slider ürünler -> tbl_products (status kolonu = 2)
        // Özel ürünler -> tbl_Products(status = 3)
        // Yıldızlı ürünler (> tbl_Products(status = 4))
        // En yeni ürünler (tbl_Products tablosunda, kayıt tarihi order by desc)
        // İndirimdeki ürünler (tbl_Products tablosunda, indirim(discount), order by desc)
        // Öne çıkanlar (tbl_Products -> tıklamasayısı)
        // Sipariş onaylandığında,(tbl_Products -> ÇokSatanlar)
        // Benzer ürünler -> 


        // sepetim
        // 10=1 & 20=1 & 30=1

        // Request -> browser'ın serverdan talebi
        // Response -> Server'ın browser'a cevabı


        iakademi_projeEntities db = new iakademi_projeEntities();

        public BaseController()
        {
            ViewBag.kategoriListesi = db.tbl_Categories.ToList();
            ViewBag.markaListesi = db.tbl_Suppliers.ToList();
        }

    }
}