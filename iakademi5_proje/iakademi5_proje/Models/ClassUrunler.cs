using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iakademi5_proje.Models
{
    public class ClassUrunler
    {

        iakademi_projeEntities db = new iakademi_projeEntities();
        public vw_aktif_urunler tek_ozel_urun_getir()
        {
            vw_aktif_urunler urun = db.vw_aktif_urunler.FirstOrDefault(u => u.status == 1);
            return urun;
        }

        public List<vw_aktif_urunler> slider_urunler_getir()
        {
            List<vw_aktif_urunler> urunler = db.vw_aktif_urunler.Where(u => u.status == 2).ToList();
            return urunler;
        }

        public static void onecikanlar_kolonunu_arttir(int id)
        {
            using (iakademi_projeEntities db2 = new iakademi_projeEntities())
            {
                tbl_Products prd = db2.tbl_Products.FirstOrDefault(u => u.productID == id);
                prd.tiklamasayisi = prd.tiklamasayisi + 1;
                db2.SaveChanges();
            }


        }

        // crtl + M + O metodları kapatır
        // ctrl + M + L metodları açar

        public List<vw_aktif_urunler> ozel_urunler_getir()
        {
            List<vw_aktif_urunler> urunler = db.vw_aktif_urunler.Where(u => u.status == 3).OrderBy(u => u.productname).Take(8).ToList();
            return urunler;
        }

        public List<vw_aktif_urunler> yildizli_urunler_getir()
        {
            List<vw_aktif_urunler> urunler = db.vw_aktif_urunler.Where(u => u.status == 4).OrderBy(u => u.price).Take(8).ToList();
            return urunler;
        }

        public List<vw_aktif_urunler> enyeni_urunler_getir()
        {
            List<vw_aktif_urunler> urunler = db.vw_aktif_urunler.OrderByDescending(u => u.adddate).Take(8).ToList();
            return urunler;
        }

        public List<vw_aktif_urunler> indirimli_urunler()
        {
            List<vw_aktif_urunler> urunler = db.vw_aktif_urunler.OrderByDescending(u => u.discount).Take(8).ToList();
            return urunler;
        }

        public List<vw_aktif_urunler> onecikanlar_getir()
        {
            List<vw_aktif_urunler> urunler = db.vw_aktif_urunler.OrderByDescending(u => u.tiklamasayisi).Take(8).ToList();
            return urunler;
        }
        
        public List<vw_aktif_urunler> coksatanlar_getir()
        {
            List<vw_aktif_urunler> urunler = db.vw_aktif_urunler.OrderByDescending(u => u.coksatanlar).Take(8).ToList();
            return urunler;
        }

        public List<vw_aktif_urunler> enyeni_urunler_getir_hepsi()
        {
            List<vw_aktif_urunler> urunler = db.vw_aktif_urunler.OrderByDescending(u => u.adddate).Take(4).ToList();
            return urunler;
        }
        public List<vw_aktif_urunler> enyeni_urunler_getir_hepsi_loadmore(int sayfano)
        {
            List<vw_aktif_urunler> urunler = db.vw_aktif_urunler.OrderByDescending(p => p.adddate).Skip(sayfano * 4).Take(4).ToList();
            return urunler;
        }

        public List<vw_aktif_urunler> ozel_urunler_getir_hepsi()
        {
            List<vw_aktif_urunler> urunler = db.vw_aktif_urunler.Where(u => u.status == 3).OrderBy(u => u.price).Take(4).ToList();
            return urunler;
        }

        public List<vw_aktif_urunler> ozel_urunler_getir_hepsi_loadmore(int sayfano)
        {
            List<vw_aktif_urunler> urunler = db.vw_aktif_urunler.Where(p => p.status == 3).OrderBy(u => u.price).Skip(sayfano * 4).Take(4).ToList();
            return urunler;
        }

        public List<vw_aktif_urunler> indirimli_urunler_getir_hepsi()
        {
            List<vw_aktif_urunler> urunler = db.vw_aktif_urunler.OrderByDescending(u => u.discount).Take(4).ToList();
            return urunler;
        }

        public List<vw_aktif_urunler> indirimli_urunler_getir_hepsi_loadmore(int sayfano)
        {
            List<vw_aktif_urunler> urunler = db.vw_aktif_urunler.OrderByDescending(p => p.discount).Skip(sayfano * 4).Take(4).ToList();
            return urunler;
        }

        public static List<view_arama> arama_getir(string urunadi)
        {
            List<view_arama> Arama = new List<view_arama>();
            using (iakademi_projeEntities db = new iakademi_projeEntities())
            {
                Arama = db.view_arama.Where(p => p.ARAMAISMI.Contains(urunadi)).Take(15).ToList();
            }
            return Arama;

        }

        public vw_aktif_urunler detayli_urun_getir(int id)
        {
            vw_aktif_urunler urn = db.vw_aktif_urunler.FirstOrDefault(u => u.productID == id);
            return urn;
        }

        public List<vw_aktif_urunler> benzer_urunler(int id)
        {
            List<vw_aktif_urunler> ulist = db.vw_aktif_urunler.Where(p => p.benzer_urunler == id).Take(4).ToList();
            return ulist;                
        }

        public List<vw_aktif_urunler> buna_bakanlar(int id)
        {
            List<vw_aktif_urunler> catlist = db.vw_aktif_urunler.Where(p => p.categoryID == id).ToList();
            return catlist;
        }

        public List<tbl_Comments> yorumlari_getir(int id)
        {
            List<tbl_Comments> ylist = db.tbl_Comments.Where(p => p.productID == id).OrderByDescending(p => p.yorumID).ToList();
            return ylist;
        }

        public static void yorumekle(int productID, string yorum)
        {
            using (iakademi_projeEntities db = new iakademi_projeEntities())
            {
                tbl_Comments com = new tbl_Comments();
                com.productID = productID;
                com.comment = yorum;

                //tbl_yasaklar yas = db.tbl_yasaklar.firstordefault(y => y.yasakmesaj.contains(yorum));

                //if (yas == null)
                //{
                //    db.tbl_Comments.Add(m);
                //    db.SaveChanges();
                //}
                
                db.tbl_Comments.Add(com);
                db.SaveChanges();
            }
        }
    }
}